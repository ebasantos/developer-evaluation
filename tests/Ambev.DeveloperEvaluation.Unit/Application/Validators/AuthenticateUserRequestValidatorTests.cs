using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validators
{
    public class AuthenticateUserRequestValidatorTests
    {
        private readonly AuthenticateUserRequestValidator _validator;

        public AuthenticateUserRequestValidatorTests()
        {
            _validator = new AuthenticateUserRequestValidator();
        }

        [Fact]
        public void Validate_WithValidRequest_ShouldPass()
        {
            // Arrange
            var request = new AuthenticateUserRequest
            {
                Email = "test@example.com",
                Password = "Test@123"
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("invalid-email")]
        [InlineData("@example.com")]
        [InlineData("test@")]
        public void Validate_WithInvalidEmail_ShouldFail(string email)
        {
            // Arrange
            var request = new AuthenticateUserRequest
            {
                Email = email,
                Password = "Test@123"
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Email");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("short")]
        public void Validate_WithInvalidPassword_ShouldFail(string password)
        {
            // Arrange
            var request = new AuthenticateUserRequest
            {
                Email = "test@example.com",
                Password = password
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Password");
        }

        [Fact]
        public void Validate_WithEmptyRequest_ShouldFailForBothFields()
        {
            // Arrange
            var request = new AuthenticateUserRequest();

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Email");
            Assert.Contains(result.Errors, e => e.PropertyName == "Password");
        }
    }
} 