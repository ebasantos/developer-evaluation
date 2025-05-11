using System;
using System.Linq;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        private readonly Guid _customerId = Guid.NewGuid();
        private readonly Guid _branchId = Guid.NewGuid();
        private readonly Guid _productId = Guid.NewGuid();

        [Fact]
        public void CreateSale_WithValidData_ShouldCreateSuccessfully()
        {
            // Arrange
            var saleNumber = "SALE-001";
            var saleDate = DateTime.UtcNow;
            var customerName = "John Doe";
            var branchName = "Branch 1";

            // Act
            var sale = new Sale(saleNumber, saleDate, _customerId, customerName, _branchId, branchName);

            // Assert
            Assert.Equal(saleNumber, sale.SaleNumber);
            Assert.Equal(saleDate, sale.SaleDate);
            Assert.Equal(_customerId, sale.CustomerId);
            Assert.Equal(customerName, sale.CustomerName);
            Assert.Equal(_branchId, sale.BranchId);
            Assert.Equal(branchName, sale.BranchName);
            Assert.False(sale.IsCancelled);
            Assert.Empty(sale.Items);
        }

        [Fact]
        public void AddItem_WithValidData_ShouldAddItemSuccessfully()
        {
            // Arrange
            var sale = CreateValidSale();
            var productName = "Product 1";
            var quantity = 5;
            var unitPrice = 100m;

            // Act
            sale.AddItem(_productId, productName, quantity, unitPrice);

            // Assert
            var item = sale.Items.Single();
            Assert.Equal(_productId, item.ProductId);
            Assert.Equal(productName, item.ProductName);
            Assert.Equal(quantity, item.Quantity);
            Assert.Equal(unitPrice, item.UnitPrice);
            Assert.Equal(0.10m, item.Discount); // 10% discount for 5 items
        }

        [Fact]
        public void AddItem_WithQuantityAbove20_ShouldThrowException()
        {
            // Arrange
            var sale = CreateValidSale();
            var productName = "Product 1";
            var quantity = 21;
            var unitPrice = 100m;

            // Act & Assert
            Assert.Throws<DomainException>(() => sale.AddItem(_productId, productName, quantity, unitPrice));
        }

        [Fact]
        public void AddItem_WithQuantityBetween10And20_ShouldApply20PercentDiscount()
        {
            // Arrange
            var sale = CreateValidSale();
            var productName = "Product 1";
            var quantity = 15;
            var unitPrice = 100m;

            // Act
            sale.AddItem(_productId, productName, quantity, unitPrice);

            // Assert
            var item = sale.Items.Single();
            Assert.Equal(0.20m, item.Discount);
        }

        [Fact]
        public void AddItem_WithQuantityBelow4_ShouldNotApplyDiscount()
        {
            // Arrange
            var sale = CreateValidSale();
            var productName = "Product 1";
            var quantity = 3;
            var unitPrice = 100m;

            // Act
            sale.AddItem(_productId, productName, quantity, unitPrice);

            // Assert
            var item = sale.Items.Single();
            Assert.Equal(0m, item.Discount);
        }

        [Fact]
        public void Cancel_ShouldMarkSaleAsCancelled()
        {
            // Arrange
            var sale = CreateValidSale();

            // Act
            sale.Cancel();

            // Assert
            Assert.True(sale.IsCancelled);
            Assert.Contains(sale.DomainEvents, e => e is SaleCancelledEvent);
        }

        [Fact]
        public void Cancel_WhenAlreadyCancelled_ShouldThrowException()
        {
            // Arrange
            var sale = CreateValidSale();
            sale.Cancel();

            // Act & Assert
            Assert.Throws<DomainException>(() => sale.Cancel());
        }

        [Fact]
        public void CancelItem_ShouldMarkItemAsCancelled()
        {
            // Arrange
            var sale = CreateValidSale();
            sale.AddItem(_productId, "Product 1", 5, 100m);
            var itemId = sale.Items.First().Id;

            // Act
            sale.CancelItem(itemId);

            // Assert
            var item = sale.Items.First();
            Assert.True(item.IsCancelled);
            Assert.Contains(sale.DomainEvents, e => e is ItemCancelledEvent);
        }

        [Fact]
        public void CancelItem_WhenItemNotFound_ShouldThrowException()
        {
            // Arrange
            var sale = CreateValidSale();

            // Act & Assert
            Assert.Throws<DomainException>(() => sale.CancelItem(Guid.NewGuid()));
        }

        private Sale CreateValidSale()
        {
            return new Sale(
                "SALE-001",
                DateTime.UtcNow,
                _customerId,
                "John Doe",
                _branchId,
                "Branch 1"
            );
        }
    }
} 