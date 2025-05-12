namespace Ambev.DeveloperEvaluation.Domain.Events.Sale
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