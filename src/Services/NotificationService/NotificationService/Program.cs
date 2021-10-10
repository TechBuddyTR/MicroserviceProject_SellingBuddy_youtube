using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.IntegrationEvents.EventHandlers;
using PaymentService.Api.IntegrationEvents.Events;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Threading.Tasks;

namespace NotificationService
{
    class Program
    {
        private static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        //static async Task Main(string[] args)
        //{
        //    var counter = 0;
        //    var max = args.Length != 0 ? Convert.ToInt32(args[0]) : -1;
        //    while (max == -1 || counter < max)
        //    {
        //        Console.WriteLine($"Counter: {++counter}");
        //        await Task.Delay(1000);
        //    }
        //}

        static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();

            ConfigureServices(services);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(serilogConfiguration)
                .CreateLogger();

            var sp = services.BuildServiceProvider();

            IEventBus eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();
            eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();


            Log.Information("Application is Running....");
            Console.WriteLine("Application is Running....");

            Console.ReadKey();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<OrderPaymentFailedIntegrationEventHandler>();
            services.AddTransient<OrderPaymentSuccessIntegrationEventHandler>();

            services.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = "IntegrationEvent",
                    SubscriberClientAppName = "NotificationService",
                    EventBusType = EventBusType.RabbitMQ,
                    Connection = new ConnectionFactory()
                    {
                        HostName = "c_rabbitmq"
                    }
                };

                return EventBusFactory.Create(config, sp);
            });
        }

        private static IConfiguration serilogConfiguration
        {
            get
            {
                return new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile($"Configurations/serilog.json", optional: false)
                    .AddJsonFile($"Configurations/serilog.{env}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();
            }
        }
    }
}
