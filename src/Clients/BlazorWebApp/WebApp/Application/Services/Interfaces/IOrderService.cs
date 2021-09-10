using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Application.Services.Dtos;
using WebApp.Domain.Models.ViewModels;

namespace WebApp.Application.Services.Interfaces
{
    public interface IOrderService
    {
        BasketDTO MapOrderToBasket(Order order);
    }
}
