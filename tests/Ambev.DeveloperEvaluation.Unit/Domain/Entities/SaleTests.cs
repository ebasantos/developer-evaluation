using System;
using System.Linq;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sale;
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
            var ex = Assert.Throws<DomainException>(() => sale.AddItem(_productId, productName, quantity, unitPrice));
            Assert.Equal("you cannot sell more than 20 same items.", ex.Message);
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
            Assert.Equal(1200m, item.TotalAmount); // 15 * 100 * 0.8 (20% discount)
        }

        [Fact]
        public void AddItem_WithQuantityBetween5And9_ShouldApply10PercentDiscount()
        {
            // Arrange
            var sale = CreateValidSale();
            var productName = "Product 1";
            var quantity = 7;
            var unitPrice = 100m;

            // Act
            sale.AddItem(_productId, productName, quantity, unitPrice);

            // Assert
            var item = sale.Items.Single();
            Assert.Equal(0.10m, item.Discount);
            Assert.Equal(630m, item.TotalAmount); // 7 * 100 * 0.9 (10% discount)
        }

        [Fact]
        public void AddItem_WithQuantityBelow5_ShouldNotApplyDiscount()
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
            Assert.Equal(300m, item.TotalAmount); // 3 * 100 (no discount)
        }

        [Fact]
        public void AddItem_WithMultipleItems_ShouldCalculateTotalAmountCorrectly()
        {
            // Arrange
            var sale = CreateValidSale();
            var product1 = Guid.NewGuid();
            var product2 = Guid.NewGuid();

            // Act
            sale.AddItem(product1, "Product 1", 15, 100m); // 20% discount
            sale.AddItem(product2, "Product 2", 7, 200m);  // 10% discount

            // Assert
            Assert.Equal(2, sale.Items.Count);
            Assert.Equal(1200m, sale.Items.First().TotalAmount); // 15 * 100 * 0.8
            Assert.Equal(1260m, sale.Items.Last().TotalAmount);  // 7 * 200 * 0.9
            Assert.Equal(2460m, sale.TotalAmount); // 1200 + 1260
        }

        [Fact]
        public void AddItem_WithZeroQuantity_ShouldThrowException()
        {
            // Arrange
            var sale = CreateValidSale();
            var productName = "Product 1";
            var quantity = 0;
            var unitPrice = 100m;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => sale.AddItem(_productId, productName, quantity, unitPrice));
            Assert.Equal("quantity must be greater than zero", ex.Message);
        }

        [Fact]
        public void AddItem_WithNegativeQuantity_ShouldThrowException()
        {
            // Arrange
            var sale = CreateValidSale();
            var productName = "Product 1";
            var quantity = -1;
            var unitPrice = 100m;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => sale.AddItem(_productId, productName, quantity, unitPrice));
            Assert.Equal("quantity must be greater than zero", ex.Message);
        }

        [Fact]
        public void AddItem_WithZeroUnitPrice_ShouldThrowException()
        {
            // Arrange
            var sale = CreateValidSale();
            var productName = "Product 1";
            var quantity = 5;
            var unitPrice = 0m;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => sale.AddItem(_productId, productName, quantity, unitPrice));
            Assert.Equal("unit price must be greater than zero", ex.Message);
        }

        [Fact]
        public void AddItem_WithNegativeUnitPrice_ShouldThrowException()
        {
            // Arrange
            var sale = CreateValidSale();
            var productName = "Product 1";
            var quantity = 5;
            var unitPrice = -100m;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => sale.AddItem(_productId, productName, quantity, unitPrice));
            Assert.Equal("unit price must be greater than zero", ex.Message);
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
            Assert.Contains(sale.GetDomainEvents(), e => e is SaleCancelledEvent);
        }

        [Fact]
        public void Cancel_WhenAlreadyCancelled_ShouldThrowException()
        {
            // Arrange
            var sale = CreateValidSale();
            sale.Cancel();

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => sale.Cancel());
            Assert.Equal("sale already canceled", ex.Message);
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
            Assert.Contains(sale.GetDomainEvents(), e => e is ItemCancelledEvent);
        }

        [Fact]
        public void CancelItem_WhenItemNotFound_ShouldThrowException()
        {
            // Arrange
            var sale = CreateValidSale();

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => sale.CancelItem(Guid.NewGuid()));
            Assert.Equal("item not found", ex.Message);
        }

        [Fact]
        public void CancelItem_WhenItemAlreadyCancelled_ShouldThrowException()
        {
            // Arrange
            var sale = CreateValidSale();
            sale.AddItem(_productId, "Product 1", 5, 100m);
            var itemId = sale.Items.First().Id;
            sale.CancelItem(itemId);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => sale.CancelItem(itemId));
            Assert.Equal("item already canceled", ex.Message);
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