using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Handlers
{
    public class UpdateSaleCommandHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly UpdateSaleCommandHandler _handler;

        public UpdateSaleCommandHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _handler = new UpdateSaleCommandHandler(_saleRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldUpdateSale()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var command = new UpdateSaleCommand
            {
                Id = saleId,
                SaleNumber = "SALE-001",
                SaleDate = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                CustomerName = "John Doe",
                BranchId = Guid.NewGuid(),
                BranchName = "Branch 1",
                Items = new List<UpdateSaleItemCommand>
                {
                    new UpdateSaleItemCommand
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 1",
                        Quantity = 5,
                        UnitPrice = 100m
                    }
                }
            };

            var existingSale = new Sale(
                "OLD-SALE-001",
                DateTime.UtcNow.AddDays(-1),
                Guid.NewGuid(),
                "Old Customer",
                Guid.NewGuid(),
                "Old Branch"
            );

            _saleRepositoryMock
                .Setup(x => x.GetByIdAsync(saleId))
                .ReturnsAsync(existingSale);

            _saleRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Sale>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _saleRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Sale>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSaleNotFound_ShouldThrowException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var command = new UpdateSaleCommand { Id = saleId };

            _saleRepositoryMock
                .Setup(x => x.GetByIdAsync(saleId))
                .ReturnsAsync((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenSaleIsCancelled_ShouldThrowException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var command = new UpdateSaleCommand { Id = saleId };

            var existingSale = new Sale(
                "SALE-001",
                DateTime.UtcNow,
                Guid.NewGuid(),
                "John Doe",
                Guid.NewGuid(),
                "Branch 1"
            );
            existingSale.Cancel();

            _saleRepositoryMock
                .Setup(x => x.GetByIdAsync(saleId))
                .ReturnsAsync(existingSale);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}