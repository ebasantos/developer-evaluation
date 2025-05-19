using System;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications
{
    public class CanCancelSaleItemSpecificationTests
    {
        private readonly CanCancelSaleItemSpecification _specification;

        public CanCancelSaleItemSpecificationTests()
        {
            _specification = new CanCancelSaleItemSpecification();
        }

        [Fact]
        public void IsSatisfiedBy_WithNonCancelledSaleAndItem_ShouldReturnTrue()
        {
            // Arrange
            var sale = new Sale(
                "SALE-001",
                DateTime.UtcNow,
                Guid.NewGuid(),
                "John Doe",
                Guid.NewGuid(),
                "Branch 1"
            );

            var productId = Guid.NewGuid();
            sale.AddItem(productId, "Product 1", 5, 100m);
            var item = sale.Items[0];

            // Act
            var result = _specification.IsSatisfiedBy((sale, item));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSatisfiedBy_WithCancelledSale_ShouldReturnFalse()
        {
            // Arrange
            var sale = new Sale(
                "SALE-002",
                DateTime.UtcNow,
                Guid.NewGuid(),
                "Jane Doe",
                Guid.NewGuid(),
                "Branch 2"
            );

            var productId = Guid.NewGuid();
            sale.AddItem(productId, "Product 2", 3, 200m);
            var item = sale.Items[0];

            sale.Cancel();

            // Act
            var result = _specification.IsSatisfiedBy((sale, item));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedBy_WithCancelledItem_ShouldReturnFalse()
        {
            // Arrange
            var sale = new Sale(
                "SALE-003",
                DateTime.UtcNow,
                Guid.NewGuid(),
                "Bob Smith",
                Guid.NewGuid(),
                "Branch 3"
            );

            var productId = Guid.NewGuid();
            sale.AddItem(productId, "Product 3", 4, 150m);
            var item = sale.Items[0];
            sale.CancelItem(productId);

            // Act
            var result = _specification.IsSatisfiedBy((sale, item));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedBy_WithNullSale_ShouldReturnFalse()
        {
            // Arrange
            var item = new SaleItem(Guid.NewGuid(), "Product 4", 2, 300m);

            // Act
            var result = _specification.IsSatisfiedBy((null, item));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedBy_WithNullItem_ShouldReturnFalse()
        {
            // Arrange
            var sale = new Sale(
                "SALE-004",
                DateTime.UtcNow,
                Guid.NewGuid(),
                "Alice Johnson",
                Guid.NewGuid(),
                "Branch 4"
            );

            // Act
            var result = _specification.IsSatisfiedBy((sale, null));

            // Assert
            Assert.False(result);
        }
    }
} 