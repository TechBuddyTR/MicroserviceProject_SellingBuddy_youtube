using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Extensions;
using BasketService.Api.Extensions.Registration;
using BasketService.Api.Infrastructure.Repository;
using BasketService.Api.IntegrationEvents.EventHanders;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.Api
{
    public class Startup
    {
        private readonly ILogger<Startup> logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            this.logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesExt(services);

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasketService.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            logger.LogInformation("System up and running - From Configure {TestParam}", "Salih");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BasketService.Api v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.RegisterWithConsul(lifetime, Configuration);

            ConfigureSubscription(app.ApplicationServices);
        }

        private void ConfigureServicesExt(IServiceCollection services)
        {
            services.ConfigureAuth(Configuration);
            services.AddSingleton(sp => sp.ConfigureRedis(Configuration));

            services.ConfigureConsul(Configuration);

            services.AddHttpContextAccessor();

            services.AddTransient<IBasketRepository, RedisBasketRepository>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = "IntegrationEvent",
                    SubscriberClientAppName = "BasketService",
                    EventBusType = EventBusType.RabbitMQ,
                    Connection = new ConnectionFactory()
                    {
                        HostName = "localhost"
                        //HostName = "localhost",
                        //Port = 15672,
                        //UserName = "guest",
                        //Password = "guest",
                        //VirtualHost="/"
                    }

                    //Connection = new ConnectionFactory()
                    //{
                    //    //HostName = "c_rabbitmq"
                    //    HostName = "http://localhost:15672"
                    //}
                };

                return EventBusFactory.Create(config, sp);
            });

            services.AddTransient<OrderCreatedIntegrationEventHandler>();
        }

        private void ConfigureSubscription(IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        }
    }
}
