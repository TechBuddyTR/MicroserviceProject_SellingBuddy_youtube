using CatalogService.Api.Extensions;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);

            hostBuilder.MigrateDbContext<CatalogContext>((context, services) =>
            {
                var env = services.GetService<IWebHostEnvironment>();
                var logger = services.GetService<ILogger<CatalogContextSeed>>();

                new CatalogContextSeed()
                    .SeedAsync(context, env, logger)
                    .Wait();
            });

            hostBuilder.Run();
        }

        static IWebHost CreateHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseWebRoot("Pics")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .Build();
        }
    }
}
