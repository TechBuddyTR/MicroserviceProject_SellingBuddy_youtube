using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Domain.Models.ViewModels
{
    public class Basket
    {
        public List<BasketItem> Items { get; init; } = new List<BasketItem>();
        public string BuyerId { get; init; }

        public decimal Total()
        {
            return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
        }
    }
}
