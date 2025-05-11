using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCancelledEvent : DomainEvent
    {
        public Guid SaleId { get; }

        public SaleCancelledEvent(Guid saleId)
        {
            SaleId = saleId;
        }
    }
} 