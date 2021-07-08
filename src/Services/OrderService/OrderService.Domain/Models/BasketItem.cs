using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Models
{
    public class BasketItem
    {
        public string Id { get; init; }

        public int ProductId { get; init; }

        public string ProductName { get; init; }

        public decimal UnitPrice { get; init; }

        public decimal OldUnitPrice { get; init; }

        public int Quantity { get; init; }

        public string PictureUrl { get; init; }
    }
}
