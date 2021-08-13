using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Domain.Models;
using WebApp.Domain.Models.CatalogModels;

namespace WebApp.Application.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<PaginatedItemsViewModel<CatalogItem>> GetCatalogItems();
    }
}
