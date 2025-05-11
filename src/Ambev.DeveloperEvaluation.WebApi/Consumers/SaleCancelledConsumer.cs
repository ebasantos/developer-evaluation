using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Consumers
{
    public class SaleCancelledConsumer : IConsumer<SaleCancelledEvent>
    {
        private readonly ILogger<SaleCancelledConsumer> _logger;

        public SaleCancelledConsumer(ILogger<SaleCancelledConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SaleCancelledEvent> context)
        {
            _logger.LogInformation("Venda cancelada: {SaleId}", context.Message.SaleId);
            return Task.CompletedTask;
        }
    }
} 