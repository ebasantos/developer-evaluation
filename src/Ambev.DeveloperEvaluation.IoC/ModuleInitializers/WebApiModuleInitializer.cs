using Ambev.DeveloperEvaluation.WebApi.Consumers.Sale;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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
                x.AddConsumer<CancelSaleItemConsumer>();
                x.AddConsumer<CancelSaleConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h => { });
                });
            });
        }
    }
}
