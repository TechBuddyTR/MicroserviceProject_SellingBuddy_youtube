using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Api.IntegrationEvents.Events
{
    public class OrderPaymentSuccessIntegrationEvent: IntegrationEvent
    {
        public Guid OrderId { get; }

        public OrderPaymentSuccessIntegrationEvent(Guid orderId) => OrderId = orderId;
    }
}
