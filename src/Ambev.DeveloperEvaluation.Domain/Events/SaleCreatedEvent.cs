using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCreatedEvent : DomainEvent
    {
        public Guid SaleId { get; }

        public SaleCreatedEvent(Guid saleId)
        {
            SaleId = saleId;
        }
    }
} 