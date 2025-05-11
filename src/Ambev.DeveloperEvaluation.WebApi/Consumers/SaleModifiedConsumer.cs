using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Consumers
{
    public class SaleModifiedConsumer : IConsumer<SaleModifiedEvent>
    {
        private readonly ILogger<SaleModifiedConsumer> _logger;

        public SaleModifiedConsumer(ILogger<SaleModifiedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SaleModifiedEvent> context)
        {
            _logger.LogInformation("Venda modificada: {SaleId}", context.Message.SaleId);
            return Task.CompletedTask;
        }
    }
} 