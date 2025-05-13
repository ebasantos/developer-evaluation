using Ambev.DeveloperEvaluation.Application.Sale.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MassTransit;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Handlers
{
    public class CancelSaleCommandHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<IBus> _busMock;
        private readonly CancelSaleCommandHandler _handler;

        public CancelSaleCommandHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _busMock = new Mock<IBus>();
            _handler = new CancelSaleCommandHandler(_saleRepositoryMock.Object, _busMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCancelSale()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var command = new CancelSaleCommand { Id = saleId };

            var existingSale = new Sale(
                "SALE-001",
                DateTime.UtcNow,
                Guid.NewGuid(),
                "John Doe",
                Guid.NewGuid(),
                "Branch 1"
            );

            _saleRepositoryMock
                .Setup(x => x.GetByIdAsync(saleId))
                .ReturnsAsync(existingSale);

            _saleRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Sale>()))
                .Returns(Task.FromResult<Sale>(null));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _saleRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Sale>()), Times.Once);
            Assert.True(existingSale.IsCancelled);
        }

        [Fact]
        public async Task Handle_WhenSaleNotFound_ShouldThrowException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var command = new CancelSaleCommand { Id = saleId };

            _saleRepositoryMock
                .Setup(x => x.GetByIdAsync(saleId))
                .ReturnsAsync((Sale)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("sale not found", ex.Message);
        }

        [Fact]
        public async Task Handle_WhenSaleIsAlreadyCancelled_ShouldThrowException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var command = new CancelSaleCommand { Id = saleId };

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
            var ex = await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("sale already canceled", ex.Message);
        }
    }
}