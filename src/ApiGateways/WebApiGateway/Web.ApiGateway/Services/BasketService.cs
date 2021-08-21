using System.Net.Http;
using System.Threading.Tasks;
using Web.ApiGateway.Extensions;
using Web.ApiGateway.Models.Basket;
using Web.ApiGateway.Services.Interfaces;

namespace Web.ApiGateway.Services
{
    public class BasketService : IBasketService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public BasketService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<BasketData> GetById(string id)
        {
            var client = httpClientFactory.CreateClient("basket");

            var response = await client.GetResponseAsync<BasketData>(id);

            return response ?? new BasketData(id);
        }

        public async Task<BasketData> UpdateAsync(BasketData currentBasket)
        {
            var client = httpClientFactory.CreateClient("basket");

            return await client.PostGetResponseAsync<BasketData, BasketData>("update", currentBasket);
        }
    }
}
