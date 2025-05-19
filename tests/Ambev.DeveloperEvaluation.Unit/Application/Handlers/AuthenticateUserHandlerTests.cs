using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Handlers
{
    public class AuthenticateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
        private readonly AuthenticateUserHandler _handler;

        public AuthenticateUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

            _handler = new AuthenticateUserHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtTokenGeneratorMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Test@123";
            var hashedPassword = "hashed_password";
            var token = "jwt_token";

            var user = new User(
                "Test User",
                email,
                hashedPassword,
                "+1234567890",
                UserRole.Customer
            );

            var command = new AuthenticateUserCommand(email, password);

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(h => h.VerifyPassword(password, hashedPassword))
                .Returns(true);

            _jwtTokenGeneratorMock
                .Setup(g => g.GenerateToken(user))
                .Returns(token);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(token, result.Token);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.Role, result.Role);
        }

        [Fact]
        public async Task Handle_WithInvalidEmail_ShouldReturnFailure()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var password = "Test@123";

            var command = new AuthenticateUserCommand(email, password);

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid credentials", result.Error);
        }

        [Fact]
        public async Task Handle_WithInvalidPassword_ShouldReturnFailure()
        {
            // Arrange
            var email = "test@example.com";
            var password = "WrongPassword";
            var hashedPassword = "hashed_password";

            var user = new User(
                "Test User",
                email,
                hashedPassword,
                "+1234567890",
                UserRole.Customer
            );

            var command = new AuthenticateUserCommand(email, password);

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(h => h.VerifyPassword(password, hashedPassword))
                .Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid credentials", result.Error);
        }

        [Fact]
        public async Task Handle_WithInactiveUser_ShouldReturnFailure()
        {
            // Arrange
            var email = "inactive@example.com";
            var password = "Test@123";
            var hashedPassword = "hashed_password";

            var user = new User(
                "Inactive User",
                email,
                hashedPassword,
                "+1234567890",
                UserRole.Customer
            );
            user.Deactivate();

            var command = new AuthenticateUserCommand(email, password);

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User is inactive", result.Error);
        }
    }
} 