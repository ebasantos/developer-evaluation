using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Security
{
    public class SecurityTests
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IConfiguration _configuration;

        public SecurityTests()
        {
            _passwordHasher = new BCryptPasswordHasher();
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("JwtSettings:SecretKey", "your-256-bit-secret-key-here"),
                    new KeyValuePair<string, string>("JwtSettings:Issuer", "test-issuer"),
                    new KeyValuePair<string, string>("JwtSettings:Audience", "test-audience"),
                    new KeyValuePair<string, string>("JwtSettings:ExpirationInMinutes", "60")
                })
                .Build();
            _jwtTokenGenerator = new JwtTokenGenerator(_configuration);
        }

        [Fact]
        public void HashPassword_ShouldCreateDifferentHashesForSamePassword()
        {
            // Arrange
            var password = "Test@123";

            // Act
            var hash1 = _passwordHasher.HashPassword(password);
            var hash2 = _passwordHasher.HashPassword(password);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            var password = "Test@123";
            var hash = _passwordHasher.HashPassword(password);

            // Act
            var result = _passwordHasher.VerifyPassword(password, hash);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
        {
            // Arrange
            var password = "Test@123";
            var hash = _passwordHasher.HashPassword(password);

            // Act
            var result = _passwordHasher.VerifyPassword("WrongPassword", hash);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GenerateToken_ShouldCreateValidJwtToken()
        {
            // Arrange
            var user = new User(
                "John Doe",
                "john@example.com",
                "Test@123",
                "+1234567890",
                UserRole.Customer
            );

            // Act
            var token = _jwtTokenGenerator.GenerateToken(user);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.Equal("test-issuer", jwtToken.Issuer);
            Assert.Equal("test-audience", jwtToken.Audiences.First());
            Assert.Equal(user.Email, jwtToken.Claims.First(c => c.Type == "email").Value);
            Assert.Equal(user.Role.ToString(), jwtToken.Claims.First(c => c.Type == "role").Value);
        }

        [Fact]
        public void GenerateToken_ShouldIncludeCorrectClaims()
        {
            // Arrange
            var user = new User(
                "Jane Doe",
                "jane@example.com",
                "Test@123",
                "+0987654321",
                UserRole.Admin
            );

            // Act
            var token = _jwtTokenGenerator.GenerateToken(user);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.Contains(jwtToken.Claims, c => c.Type == "email" && c.Value == user.Email);
            Assert.Contains(jwtToken.Claims, c => c.Type == "role" && c.Value == user.Role.ToString());
            Assert.Contains(jwtToken.Claims, c => c.Type == "name" && c.Value == user.Name);
            Assert.Contains(jwtToken.Claims, c => c.Type == "sub" && c.Value == user.Id.ToString());
        }

        [Fact]
        public void GenerateToken_ShouldSetCorrectExpiration()
        {
            // Arrange
            var user = new User(
                "Test User",
                "test@example.com",
                "Test@123",
                "+1122334455",
                UserRole.Customer
            );

            // Act
            var token = _jwtTokenGenerator.GenerateToken(user);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var expectedExpiration = DateTime.UtcNow.AddMinutes(60);
            var actualExpiration = jwtToken.ValidTo;

            // Allow for a small time difference due to test execution
            Assert.True(Math.Abs((expectedExpiration - actualExpiration).TotalMinutes) < 1);
        }
    }
} 