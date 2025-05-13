using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validators
{
    public class CancelSaleItemRequestValidatorTests
    {
        private readonly CancelSaleItemRequestValidator _validator;

        public CancelSaleItemRequestValidatorTests()
        {
            _validator = new CancelSaleItemRequestValidator();
        }

        [Fact]
        public void Validate_WithValidRequest_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var request = new CancelSaleItemRequest
            {
                SaleId = Guid.NewGuid(),
                ItemId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.SaleId);
            result.ShouldNotHaveValidationErrorFor(x => x.ItemId);
        }

        [Fact]
        public void Validate_WithEmptySaleId_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CancelSaleItemRequest
            {
                SaleId = Guid.Empty,
                ItemId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SaleId);
        }

        [Fact]
        public void Validate_WithEmptyItemId_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CancelSaleItemRequest
            {
                SaleId = Guid.NewGuid(),
                ItemId = Guid.Empty
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ItemId);
        }
    }
}