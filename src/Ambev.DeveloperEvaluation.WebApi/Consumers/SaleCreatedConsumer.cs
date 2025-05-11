using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Consumers
{
    public class SaleCreatedConsumer : IConsumer<SaleCreatedEvent>
    {
        private readonly ILogger<SaleCreatedConsumer> _logger;

        public SaleCreatedConsumer(ILogger<SaleCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SaleCreatedEvent> context)
        {
            _logger.LogInformation("Venda criada: {SaleId}", context.Message.SaleId);
            return Task.CompletedTask;
        }
    }
} 