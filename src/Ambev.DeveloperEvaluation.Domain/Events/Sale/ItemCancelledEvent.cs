namespace Ambev.DeveloperEvaluation.Domain.Events.Sale
{
    public class ItemCancelledEvent : DomainEvent
    {
        public Guid SaleId { get; }
        public Guid ItemId { get; }

        public ItemCancelledEvent(Guid saleId, Guid itemId)
        {
            SaleId = saleId;
            ItemId = itemId;
        }
    }
}