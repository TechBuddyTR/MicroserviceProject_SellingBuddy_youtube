using MediatR;
using OrderService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Features.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<bool>
    {
        
        private readonly List<OrderItemDTO> _orderItems;

        
        
        public string UserName { get; private set; }

        
        public string City { get; private set; }

        
        public string Street { get; private set; }

        
        public string State { get; private set; }

        
        public string Country { get; private set; }

        
        public string ZipCode { get; private set; }

        
        public string CardNumber { get; private set; }

        
        public string CardHolderName { get; private set; }

        
        public DateTime CardExpiration { get; private set; }

        
        public string CardSecurityNumber { get; private set; }

        
        public int CardTypeId { get; private set; }

        
        public IEnumerable<OrderItemDTO> OrderItems => _orderItems;

        public CreateOrderCommand()
        {
            _orderItems = new List<OrderItemDTO>();
        }


        public CreateOrderCommand(List<BasketItem> basketItems, string userId, string userName, string city, string street, string state, string country, string zipcode,
            string cardNumber, string cardHolderName, DateTime cardExpiration,
            string cardSecurityNumber, int cardTypeId) : this()
        {
            var dtoList = basketItems.Select(item => new OrderItemDTO()
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                PictureUrl = item.PictureUrl,
                UnitPrice = item.UnitPrice,
                Units = item.Quantity
            });

            _orderItems = dtoList.ToList();

            UserName = userName;
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipcode;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            CardSecurityNumber = cardSecurityNumber;
            CardTypeId = cardTypeId;
            CardExpiration = cardExpiration;
        }
    }

    public class OrderItemDTO
    {
        public int ProductId { get; init; }

        public string ProductName { get; init; }

        public decimal UnitPrice { get; init; }

        public int Units { get; init; }

        public string PictureUrl { get; init; }
    }
}
