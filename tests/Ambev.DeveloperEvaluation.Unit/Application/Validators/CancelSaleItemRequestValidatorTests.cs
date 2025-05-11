using System;
using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Application.Validators;
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
        public void Validate_WithValidRequest_ShouldNotHaveErrors()
        {
            // Arrange
            var request = new CancelSaleItemRequest
            {
                Reason = "Produto indisponível"
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
            var request = new CancelSaleItemRequest
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
            var request = new CancelSaleItemRequest
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