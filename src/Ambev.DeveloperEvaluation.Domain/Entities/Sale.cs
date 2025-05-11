using System;
using System.Collections.Generic;
using System.Linq;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;

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
        private readonly List<DomainEvent> _domainEvents = new();

        protected Sale() { }

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
                throw new DomainException("Não é possível vender mais de 20 itens idênticos.");

            var discount = CalculateDiscount(quantity);
            var item = new SaleItem(productId, productName, quantity, unitPrice, discount);
            _items.Add(item);
            
            RecalculateTotal();
            AddDomainEvent(new SaleModifiedEvent(Id));
        }

        private decimal CalculateDiscount(int quantity)
        {
            if (quantity < 4) return 0;
            if (quantity >= 10 && quantity <= 20) return 0.20m;
            return 0.10m;
        }

        private void RecalculateTotal()
        {
            TotalAmount = _items.Sum(item => item.TotalAmount);
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new DomainException("Venda já está cancelada.");

            IsCancelled = true;
            AddDomainEvent(new SaleCancelledEvent(Id));
        }

        public void CancelItem(Guid itemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new DomainException("Item não encontrado na venda.");

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