using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validators
{
    public class UpdateSaleRequestValidatorTests
    {
        private readonly UpdateSaleRequestValidator _validator;

        public UpdateSaleRequestValidatorTests()
        {
            _validator = new UpdateSaleRequestValidator();
        }

        [Fact]
        public void Validate_WithValidRequest_ShouldNotHaveErrors()
        {
            // Arrange
            var request = new UpdateSaleRequest
            {
                Id = Guid.NewGuid(),
                SaleNumber = "SALE-001",
                SaleDate = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                CustomerName = "John Doe",
                BranchId = Guid.NewGuid(),
                BranchName = "Branch 1",
                Items = new[]
                {
                    new UpdateSaleItemRequest
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 1",
                        Quantity = 5,
                        UnitPrice = 100m
                    }
                }.ToList()
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WithEmptySaleNumber_ShouldHaveError()
        {
            // Arrange
            var request = new UpdateSaleRequest
            {
                Id = Guid.NewGuid(),
                SaleNumber = string.Empty,
                SaleDate = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                CustomerName = "John Doe",
                BranchId = Guid.NewGuid(),
                BranchName = "Branch 1",
                Items = new[]
                {
                    new UpdateSaleItemRequest
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 1",
                        Quantity = 5,
                        UnitPrice = 100m
                    }
                }.ToList()
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SaleNumber);
        }

        [Fact]
        public void Validate_WithEmptyCustomerName_ShouldHaveError()
        {
            // Arrange
            var request = new UpdateSaleRequest
            {
                Id = Guid.NewGuid(),
                SaleNumber = "SALE-001",
                SaleDate = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                CustomerName = string.Empty,
                BranchId = Guid.NewGuid(),
                BranchName = "Branch 1",
                Items = new[]
                {
                    new UpdateSaleItemRequest
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 1",
                        Quantity = 5,
                        UnitPrice = 100m
                    }
                }.ToList()
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CustomerName);
        }

        [Fact]
        public void Validate_WithEmptyBranchName_ShouldHaveError()
        {
            // Arrange
            var request = new UpdateSaleRequest
            {
                Id = Guid.NewGuid(),
                SaleNumber = "SALE-001",
                SaleDate = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                CustomerName = "John Doe",
                BranchId = Guid.NewGuid(),
                BranchName = string.Empty,
                Items = new[]
                {
                    new UpdateSaleItemRequest
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 1",
                        Quantity = 5,
                        UnitPrice = 100m
                    }
                }.ToList()
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BranchName);
        }

        [Fact]
        public void Validate_WithEmptyItems_ShouldHaveError()
        {
            // Arrange
            var request = new UpdateSaleRequest
            {
                Id = Guid.NewGuid(),
                SaleNumber = "SALE-001",
                SaleDate = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                CustomerName = "John Doe",
                BranchId = Guid.NewGuid(),
                BranchName = "Branch 1",
                Items = Array.Empty<UpdateSaleItemRequest>().ToList()
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Items);
        }

        [Fact]
        public void Validate_WithInvalidItemQuantity_ShouldHaveError()
        {
            // Arrange
            var request = new UpdateSaleRequest
            {
                Id = Guid.NewGuid(),
                SaleNumber = "SALE-001",
                SaleDate = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                CustomerName = "John Doe",
                BranchId = Guid.NewGuid(),
                BranchName = "Branch 1",
                Items = new[]
                {
                    new UpdateSaleItemRequest
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 1",
                        Quantity = 0,
                        UnitPrice = 100m
                    }
                }.ToList()
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor("Items[0].Quantity");
        }

        [Fact]
        public void Validate_WithInvalidItemUnitPrice_ShouldHaveError()
        {
            // Arrange
            var request = new UpdateSaleRequest
            {
                Id = Guid.NewGuid(),
                SaleNumber = "SALE-001",
                SaleDate = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                CustomerName = "John Doe",
                BranchId = Guid.NewGuid(),
                BranchName = "Branch 1",
                Items = new[]
                {
                    new UpdateSaleItemRequest
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 1",
                        Quantity = 5,
                        UnitPrice = 0m
                    }
                }.ToList()
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor("Items[0].UnitPrice");
        }
    }
}