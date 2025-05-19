using System;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications
{
    public class CanCancelSaleSpecificationTests
    {
        private readonly CanCancellSaleSpecification _specification;

        public CanCancelSaleSpecificationTests()
        {
            _specification = new CanCancellSaleSpecification();
        }

        [Fact]
        public void IsSatisfiedBy_WithNonCancelledSale_ShouldReturnTrue()
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

            // Act
            var result = _specification.IsSatisfiedBy(sale);

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
            sale.Cancel();

            // Act
            var result = _specification.IsSatisfiedBy(sale);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedBy_WithNullSale_ShouldReturnFalse()
        {
            // Act
            var result = _specification.IsSatisfiedBy(null);

            // Assert
            Assert.False(result);
        }
    }
} 