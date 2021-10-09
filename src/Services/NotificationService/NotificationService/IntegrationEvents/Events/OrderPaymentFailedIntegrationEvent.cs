using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Api.IntegrationEvents.Events
{
    public class OrderPaymentFailedIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; }

        public string ErrorMessage { get; }

        public OrderPaymentFailedIntegrationEvent(Guid orderId, string errorMessage)
        {
            OrderId = orderId;
            ErrorMessage = errorMessage;
        }
    }
}
