using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application
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
            _logger.LogInformation("sale updated: {SaleId}", context.Message.SaleId);
            return Task.CompletedTask;
        }
    }
}