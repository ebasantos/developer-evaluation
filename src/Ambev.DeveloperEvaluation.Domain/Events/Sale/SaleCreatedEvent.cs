namespace Ambev.DeveloperEvaluation.Domain.Events.Sale
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