using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Application.Services.Interfaces;
using WebApp.Domain.Models;
using WebApp.Domain.Models.CatalogModels;
using WebApp.Extensions;

namespace WebApp.Application.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient apiClient;

        public CatalogService(HttpClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<PaginatedItemsViewModel<CatalogItem>> GetCatalogItems()
        {
            var response = await apiClient.GetResponseAsync<PaginatedItemsViewModel<CatalogItem>>("/catalog/items");

            return response;
        }
    }
}
