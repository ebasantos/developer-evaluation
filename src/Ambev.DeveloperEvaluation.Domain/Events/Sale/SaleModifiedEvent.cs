namespace Ambev.DeveloperEvaluation.Domain.Events.Sale
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