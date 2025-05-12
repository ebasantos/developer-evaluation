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
        public void Validate_WithValidRequest_ShouldNotHaveErrors()
        {
            // Arrange
            var request = new CancelSaleRequest
            {
                Reason = "Erro no pedido"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WithEmptyReason_ShouldHaveError()
        {
            // Arrange
            var request = new CancelSaleRequest
            {
                Reason = string.Empty
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Reason);
        }

        [Fact]
        public void Validate_WithNullReason_ShouldHaveError()
        {
            // Arrange
            var request = new CancelSaleRequest
            {
                Reason = null
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Reason);
        }
    }
}