using BasketService.Api.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.Api.Core.Application.Repository
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string customerId);

        IEnumerable<string> GetUsers();

        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        
        Task<bool> DeleteBasketAsync(string id);
    }
}
