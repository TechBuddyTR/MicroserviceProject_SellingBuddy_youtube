using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.IntegrationEvents
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public string UserName { get; set; }

        public Guid OrderId { get; set; }

        public OrderStartedIntegrationEvent(string userName, Guid orderId)
        {
            UserName = userName;
            OrderId = orderId;
        }
    }
}
