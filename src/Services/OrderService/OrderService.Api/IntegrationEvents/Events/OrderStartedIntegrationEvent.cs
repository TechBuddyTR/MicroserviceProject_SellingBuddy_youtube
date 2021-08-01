using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Api.IntegrationEvents.Events
{
    public class OrderStartedIntegrationEvent: IntegrationEvent
    {
        public OrderStartedIntegrationEvent(string userId, int orderId)
        {
            UserId = userId;
            OrderId = orderId;
        }

        public string UserId { get; private set; }

        public int OrderId { get; private set; }

    }
}
