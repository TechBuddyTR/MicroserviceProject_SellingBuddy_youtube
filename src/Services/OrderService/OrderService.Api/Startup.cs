using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OrderService.Api.Extensions.Registration;
using OrderService.Api.Extensions.Registration.Consul;
using OrderService.Api.IntegrationEvents.EventHandlers;
using OrderService.Api.IntegrationEvents.Events;
using OrderService.Application;
using OrderService.Persistence;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderService.Api", Version = "v1" });
            });

            ConfigureService(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderService.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ConfigureEventBusForSubscription(app);
        }

        private void ConfigureService(IServiceCollection services)
        {
            services
                .AddLogging(configure => configure.AddConsole())
                .AddApplicationRegistration(typeof(Startup))
                .AddPersistenceRegistration(Configuration)
                .ConfigureEventHandlers()
                .AddServiceDiscoveryRegistration(Configuration)
                ;

            services.AddSingleton(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = "IntegrationEvent",
                    SubscriberClientAppName = "OrderService",
                    Connection = new ConnectionFactory()
                    {
                        HostName = "localhost",
                        Port = 15672,
                        UserName = "guest",
                        Password = "guest"
                    },
                    //Connection = new ConnectionFactory()
                    //{
                    //    //HostName = "c_rabbitmq"
                    //    HostName = "http://localhost:15672"
                    //},
                    EventBusType = EventBusType.RabbitMQ,

                };

                return EventBusFactory.Create(config, sp);
            });
        }

        private void ConfigureEventBusForSubscription(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        }
    }
}
