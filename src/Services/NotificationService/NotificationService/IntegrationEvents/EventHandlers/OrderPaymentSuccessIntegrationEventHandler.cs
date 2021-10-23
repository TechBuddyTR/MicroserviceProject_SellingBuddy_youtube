using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using PaymentService.Api.IntegrationEvents.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.IntegrationEvents.EventHandlers
{
    class OrderPaymentSuccessIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSuccessIntegrationEvent>
    {

        public Task Handle(OrderPaymentSuccessIntegrationEvent @event)
        {
            // Send Fail Notification (Sms, EMail, Push)

            Log.Logger.Information($"Order Payment Success with OrderId: {@event.OrderId}");

            return Task.CompletedTask;
        }
    }
}
