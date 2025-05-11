using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Consumers
{
    public class ItemCancelledConsumer : IConsumer<ItemCancelledEvent>
    {
        private readonly ILogger<ItemCancelledConsumer> _logger;

        public ItemCancelledConsumer(ILogger<ItemCancelledConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ItemCancelledEvent> context)
        {
            _logger.LogInformation("Item cancelado: SaleId={SaleId}, ItemId={ItemId}", 
                context.Message.SaleId, context.Message.ItemId);
            return Task.CompletedTask;
        }
    }
} 