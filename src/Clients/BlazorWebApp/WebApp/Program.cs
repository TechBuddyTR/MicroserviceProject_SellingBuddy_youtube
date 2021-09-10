using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WebApp.Utils;
using WebApp.Application.Services.Interfaces;
using WebApp.Application.Services;
using WebApp.Infrastructure;

namespace WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddBlazoredLocalStorage();



            builder.Services.AddTransient<IIdentityService, IdentityService>();
            builder.Services.AddTransient<ICatalogService, CatalogService>();
            builder.Services.AddTransient<IBasketService, BasketService>();
            builder.Services.AddTransient<IOrderService, OrderService>();

            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            builder.Services.AddSingleton<AppStateManager>();

            builder.Services.AddScoped(sp => 
            {
                var clientFactory = sp.GetRequiredService<IHttpClientFactory>();

                return clientFactory.CreateClient("ApiGatewayHttpClient");
            });


            builder.Services.AddScoped<AuthTokenHandler>();

            builder.Services.AddHttpClient("ApiGatewayHttpClient", client => 
            {
                client.BaseAddress = new Uri("http://localhost:5000/");
            })
            .AddHttpMessageHandler<AuthTokenHandler>();



            await builder.Build().RunAsync();
        }
    }
}
