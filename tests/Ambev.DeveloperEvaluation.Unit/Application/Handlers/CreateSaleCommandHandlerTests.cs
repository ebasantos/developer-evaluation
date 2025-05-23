using Ambev.DeveloperEvaluation.Application.Sale.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MassTransit;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Handlers
{
    public class CreateSaleCommandHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<IBus> _busMock;
        private readonly CreateSaleCommandHandler _handler;

        public CreateSaleCommandHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _busMock = new Mock<IBus>();
            _handler = new CreateSaleCommandHandler(_saleRepositoryMock.Object, _busMock.Object);
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
                .Returns(Task.FromResult<Sale>(null));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _saleRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Sale>()), Times.Once);
            Assert.NotEqual(Guid.Empty, result.Id);
        }
    }
}