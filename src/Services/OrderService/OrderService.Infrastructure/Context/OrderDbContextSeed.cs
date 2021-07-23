using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Domain.SeedWork;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Context;

namespace OrderService.Persistence.Context
{
    public class OrderDbContextSeed
    {
        public async Task SeedAsync(OrderDbContext context, ILogger<OrderDbContext> logger)
        {
            var policy = CreatePolicy(logger, nameof(OrderDbContextSeed));

            await policy.ExecuteAsync(async () =>
            {

                var useCustomizationData = false;
                var contentRootPath = "Seeding/Setup";

                using (context)
                {
                    context.Database.Migrate();

                    if (!context.CardTypes.Any())
                    {
                        context.CardTypes.AddRange(useCustomizationData
                                                ? GetCardTypesFromFile(contentRootPath, logger)
                                                : GetPredefinedCardTypes());

                        await context.SaveChangesAsync();
                    }

                    if (!context.OrderStatus.Any())
                    {
                        context.OrderStatus.AddRange(useCustomizationData
                                                ? GetOrderStatusFromFile(contentRootPath, logger)
                                                : GetPredefinedOrderStatus());
                    }

                    await context.SaveChangesAsync();
                }
            });
        }

        private IEnumerable<CardType> GetCardTypesFromFile(string contentRootPath, ILogger<OrderDbContext> log)
        {
            string fileName = "CardTypes.txt";

            if (!File.Exists(fileName))
            {
                return GetPredefinedCardTypes();
            }

            var fileContent = File.ReadAllLines(fileName);

            int id = 1;
            var list = fileContent.Select(i => new CardType(id++, i)).Where(i => i != null);

            return list;
        }

        private IEnumerable<CardType> GetPredefinedCardTypes()
        {
            return Enumeration.GetAll<CardType>();
        }

        private IEnumerable<OrderStatus> GetOrderStatusFromFile(string contentRootPath, ILogger<OrderDbContext> log)
        {
            string fileName = "OrderStatus.txt";

            if (!File.Exists(fileName))
            {
                return GetPredefinedOrderStatus();
            }

            var fileContent = File.ReadAllLines(fileName);

            int id = 1;
            var list = fileContent.Select(i => new OrderStatus(id++, i)).Where(i => i != null);

            return list;
        }

        private IEnumerable<OrderStatus> GetPredefinedOrderStatus()
        {
            return new List<OrderStatus>()
            {
                OrderStatus.Submitted,
                OrderStatus.AwaitingValidation,
                OrderStatus.StockConfirmed,
                OrderStatus.Paid,
                OrderStatus.Shipped,
                OrderStatus.Cancelled
            };
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<OrderDbContext> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}
