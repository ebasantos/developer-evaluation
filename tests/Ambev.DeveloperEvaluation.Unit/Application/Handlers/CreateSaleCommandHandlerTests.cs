using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Commands;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Handlers
{
    public class CreateSaleCommandHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly CreateSaleCommandHandler _handler;

        public CreateSaleCommandHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _handler = new CreateSaleCommandHandler(_saleRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreateSale()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleNumber = "SALE-001",
                SaleDate = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                CustomerName = "John Doe",
                BranchId = Guid.NewGuid(),
                BranchName = "Branch 1",
                Items = new List<CreateSaleItemCommand>
                {
                    new CreateSaleItemCommand
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 1",
                        Quantity = 5,
                        UnitPrice = 100m
                    }
                }
            };

            _saleRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Sale>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _saleRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Sale>()), Times.Once);
            Assert.NotEqual(Guid.Empty, result);
        }
    }
} 