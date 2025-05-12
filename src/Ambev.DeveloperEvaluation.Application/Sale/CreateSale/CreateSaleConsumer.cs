using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application
{
    public class CreateSaleConsumer : IConsumer<SaleCreatedEvent>
    {
        private readonly ILogger<CreateSaleConsumer> _logger;
        private readonly IMediator _mediator;

        public CreateSaleConsumer(ILogger<CreateSaleConsumer> logger, IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SaleCreatedEvent> context)
        {
            _logger.LogInformation("sale created: {SaleId}", context.Message.SaleId);

            _mediator.Publish(context.Message);

            return Task.CompletedTask;
        }
    }
}