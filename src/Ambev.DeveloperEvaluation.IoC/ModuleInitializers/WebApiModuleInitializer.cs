using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Domain.Events.Sale;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers
{
    public class WebApiModuleInitializer : IModuleInitializer
    {
        public void Initialize(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateSaleConsumer>();
                x.AddConsumer<SaleModifiedConsumer>();
                x.AddConsumer<CancelSaleConsumer>();
                x.AddConsumer<CancelSaleItemConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var host = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
                    var username = builder.Configuration["RabbitMQ:Username"] ?? "guest";
                    var password = builder.Configuration["RabbitMQ:Password"] ?? "guest";

                    cfg.Host(host, "/", h => 
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
