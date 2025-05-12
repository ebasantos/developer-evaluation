using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public string SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }
        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; }
        public Guid BranchId { get; private set; }
        public string BranchName { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        private readonly List<SaleItem> _items = new();

        protected Sale() { }


        private void CancelSale() => IsCancelled = true;

        public Sale(string saleNumber, DateTime saleDate, Guid customerId, string customerName,
            Guid branchId, string branchName)
        {
            SaleNumber = saleNumber;
            SaleDate = saleDate;
            CustomerId = customerId;
            CustomerName = customerName;
            BranchId = branchId;
            BranchName = branchName;
            IsCancelled = false;
        }

        public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            if (quantity > 20)
                throw new DomainException("you cannot sell more than 20 same items.");

            var discount = CalculateDiscount(quantity);
            var item = new SaleItem(productId, productName, quantity, unitPrice, discount);
            _items.Add(item);

            RecalculateTotal();
            AddDomainEvent(new SaleModifiedEvent(Id));
        }

        private decimal CalculateDiscount(int quantity)
        {
            return quantity switch
            {
                < 4 => DISCOUNT_UNDER_4_ITENS,
                >= 10 and <= 20 => DISCOUNT_BETWEEN_10_AND_20_ITENS,
                _ => DISCOUNT_ABOVE_4_ITENS,
            };
        }

        private const decimal DISCOUNT_UNDER_4_ITENS = 0;
        private const decimal DISCOUNT_ABOVE_4_ITENS = 0.10m;
        private const decimal DISCOUNT_BETWEEN_10_AND_20_ITENS = 0.20m;

        private void RecalculateTotal()
        {
            TotalAmount = _items.Sum(item => item.TotalAmount);
        }

        public void Cancel()
        {
            if (new CanCancellSaleSpecification().IsSatisfiedBy(this))
                throw new DomainException("sale already canceled");

            CancelSale();
            AddDomainEvent(new SaleCancelledEvent(Id));
        }

        public void CancelItem(Guid itemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new DomainException("item not found for this sale");

            item.Cancel();
            RecalculateTotal();
            AddDomainEvent(new ItemCancelledEvent(Id, itemId));
        }

        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public IReadOnlyCollection<DomainEvent> GetDomainEvents()
        {
            return _domainEvents.AsReadOnly();
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}