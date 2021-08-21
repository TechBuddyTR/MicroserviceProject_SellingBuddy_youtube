using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Application.Services.Dtos;
using WebApp.Domain.Models.ViewModels;

namespace WebApp.Application.Services.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetBasket();

        Task<Basket> UpdateBasket(Basket basket);

        Task AddItemToBasket(int productId);

        Task Checkout(BasketDTO basket);
    }
}
