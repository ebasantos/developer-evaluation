using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application
{
    public class CancelSaleItemConsumer : IConsumer<ItemCancelledEvent>
    {
        private readonly ILogger<CancelSaleItemConsumer> _logger;
        private readonly IMediator _mediator;

        public CancelSaleItemConsumer(ILogger<CancelSaleItemConsumer> logger,
            IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ItemCancelledEvent> context)
        {
            _logger.LogInformation("Item canceled: SaleId={SaleId}, ItemId={ItemId}",
                context.Message.SaleId, context.Message.ItemId);

            _mediator.Publish(context.Message);

            return Task.CompletedTask;
        }
    }
}