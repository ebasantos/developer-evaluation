using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public bool IsCancelled { get; private set; }
        public decimal TotalAmount => Quantity * UnitPrice * (1 - Discount);
        public virtual Sale Sale { get; private set; }

        protected SaleItem() { }

        private void CancelItem() => IsCancelled = true;

        public SaleItem(Guid productId, string productName, int quantity, decimal unitPrice, decimal discount)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discount;
            IsCancelled = false;
        }

        public void Cancel()
        {
            if (new CanCancelSaleItemSpecification().IsSatisfiedBy(this))
                throw new DomainException("Item already canceled.");

            CancelItem();
        }
    }
}