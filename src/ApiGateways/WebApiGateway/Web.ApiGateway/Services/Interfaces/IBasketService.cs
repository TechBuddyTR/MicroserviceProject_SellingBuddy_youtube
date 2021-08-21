using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ApiGateway.Models.Basket;

namespace Web.ApiGateway.Services.Interfaces
{
    public interface IBasketService
    {
        Task<BasketData> GetById(string id);

        Task<BasketData> UpdateAsync(BasketData currentBasket);
    }
}
