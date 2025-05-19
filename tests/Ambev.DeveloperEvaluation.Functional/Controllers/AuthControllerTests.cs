using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Controllers
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var request = new AuthenticateUserRequest
            {
                Email = "admin@example.com",
                Password = "Admin@123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<AuthenticateUserResponse>();
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
            Assert.NotEmpty(result.Token);
        }

        [Theory]
        [InlineData("", "Password123")]
        [InlineData("invalid-email", "Password123")]
        [InlineData("test@example.com", "")]
        [InlineData("test@example.com", "short")]
        public async Task Login_WithInvalidData_ShouldReturnBadRequest(string email, string password)
        {
            // Arrange
            var request = new AuthenticateUserRequest
            {
                Email = email,
                Password = password
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithNonExistingUser_ShouldReturnUnauthorized()
        {
            // Arrange
            var request = new AuthenticateUserRequest
            {
                Email = "nonexistent@example.com",
                Password = "Test@123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithWrongPassword_ShouldReturnUnauthorized()
        {
            // Arrange
            var request = new AuthenticateUserRequest
            {
                Email = "admin@example.com",
                Password = "WrongPassword@123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithInactiveUser_ShouldReturnUnauthorized()
        {
            // Arrange
            var request = new AuthenticateUserRequest
            {
                Email = "inactive@example.com",
                Password = "Test@123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
} 