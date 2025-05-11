using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleModifiedEvent : DomainEvent
    {
        public Guid SaleId { get; }

        public SaleModifiedEvent(Guid saleId)
        {
            SaleId = saleId;
        }
    }
} 