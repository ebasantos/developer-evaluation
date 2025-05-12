using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application
{
    public class CancelSaleConsumer : IConsumer<SaleCancelledEvent>
    {
        private readonly ILogger<CancelSaleConsumer> _logger;
        private readonly IMediator _mediator;

        public CancelSaleConsumer(ILogger<CancelSaleConsumer> logger, IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SaleCancelledEvent> context)
        {
            _logger.LogInformation("sale canceled: {SaleId}", context.Message.SaleId);

            _mediator.Publish(context.Message);

            return Task.CompletedTask;
        }
    }
}