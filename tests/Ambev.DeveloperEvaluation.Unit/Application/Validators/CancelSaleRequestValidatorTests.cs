using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validators
{
    public class CancelSaleRequestValidatorTests
    {
        private readonly CancelSaleRequestValidator _validator;

        public CancelSaleRequestValidatorTests()
        {
            _validator = new CancelSaleRequestValidator();
        }

        [Fact]
        public void Validate_WithValidRequest_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var request = new CancelSaleRequest(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Validate_WithEmptyId_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CancelSaleRequest(Guid.Empty);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}