using Ambev.DeveloperEvaluation.Common.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MassTransit;
using Ambev.DeveloperEvaluation.WebApi.Consumers;

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
                x.AddConsumer<SaleCreatedConsumer>();
                x.AddConsumer<SaleModifiedConsumer>();
                x.AddConsumer<SaleCancelledConsumer>();
                x.AddConsumer<ItemCancelledConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h => { });
                });
            });
        }
    }
}
