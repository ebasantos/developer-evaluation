using System;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly DefaultContext _context;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DefaultContext(options);
            _userRepository = new UserRepository(_context);
        }

        [Fact]
        public async Task CreateAsync_ShouldPersistUser()
        {
            // Arrange
            var user = new User(
                "John Doe",
                "john@example.com",
                "hashed_password",
                "+1234567890",
                UserRole.Customer
            );

            // Act
            await _userRepository.CreateAsync(user);
            await _context.SaveChangesAsync();

            // Assert
            var persistedUser = await _context.Users.FindAsync(user.Id);
            Assert.NotNull(persistedUser);
            Assert.Equal(user.Name, persistedUser.Name);
            Assert.Equal(user.Email, persistedUser.Email);
            Assert.Equal(user.Password, persistedUser.Password);
            Assert.Equal(user.Phone, persistedUser.Phone);
            Assert.Equal(user.Role, persistedUser.Role);
            Assert.Equal(user.Status, persistedUser.Status);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingUser_ShouldReturnUser()
        {
            // Arrange
            var user = new User(
                "Jane Doe",
                "jane@example.com",
                "hashed_password",
                "+0987654321",
                UserRole.Admin
            );
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByIdAsync(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistingUser_ShouldReturnNull()
        {
            // Act
            var result = await _userRepository.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByEmailAsync_WithExistingEmail_ShouldReturnUser()
        {
            // Arrange
            var email = "bob@example.com";
            var user = new User(
                "Bob Smith",
                email,
                "hashed_password",
                "+1122334455",
                UserRole.Customer
            );
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByEmailAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_WithNonExistingEmail_ShouldReturnNull()
        {
            // Act
            var result = await _userRepository.GetByEmailAsync("nonexistent@example.com");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser()
        {
            // Arrange
            var user = new User(
                "Alice Johnson",
                "alice@example.com",
                "hashed_password",
                "+9988776655",
                UserRole.Customer
            );
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            user.UpdateName("Alice Smith");
            user.UpdatePhone("+5544332211");
            await _userRepository.UpdateAsync(user);
            await _context.SaveChangesAsync();

            // Assert
            var updatedUser = await _context.Users.FindAsync(user.Id);
            Assert.NotNull(updatedUser);
            Assert.Equal("Alice Smith", updatedUser.Name);
            Assert.Equal("+5544332211", updatedUser.Phone);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveUser()
        {
            // Arrange
            var user = new User(
                "Charlie Brown",
                "charlie@example.com",
                "hashed_password",
                "+1357924680",
                UserRole.Customer
            );
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            await _userRepository.DeleteAsync(user);
            await _context.SaveChangesAsync();

            // Assert
            var deletedUser = await _context.Users.FindAsync(user.Id);
            Assert.Null(deletedUser);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
} 