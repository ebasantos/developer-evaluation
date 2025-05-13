using Ambev.DeveloperEvaluation.Application.Sale.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Handlers
{
    public class CancelSaleItemCommandHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CancelSaleItemCommandHandler _handler;

        public CancelSaleItemCommandHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CancelSaleItemCommandHandler(_saleRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCancelItem()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var command = new CancelSaleItemCommand { SaleId = saleId, ItemId = itemId };

            var existingSale = new Sale(
                "SALE-001",
                DateTime.UtcNow,
                Guid.NewGuid(),
                "John Doe",
                Guid.NewGuid(),
                "Branch 1"
            );

            existingSale.AddItem(Guid.NewGuid(), "Product 1", 5, 100m);
            var item = existingSale.Items.First();
            item.GetType().GetProperty("Id").SetValue(item, itemId);

            _saleRepositoryMock
                .Setup(x => x.GetByIdAsync(saleId))
                .ReturnsAsync(existingSale);

            _saleRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Sale>()))
                .Returns(Task.FromResult<Sale>(null));

            _mapperMock
                .Setup(x => x.Map<CancelSaleItemResult>(It.IsAny<CancelSaleItemCommand>()))
                .Returns(new CancelSaleItemResult { SaleId = saleId, ItemId = itemId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _saleRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Sale>()), Times.Once);
            Assert.True(item.IsCancelled);
            Assert.Equal(saleId, result.SaleId);
            Assert.Equal(itemId, result.ItemId);
        }

        [Fact]
        public async Task Handle_WhenSaleNotFound_ShouldThrowException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var command = new CancelSaleItemCommand { SaleId = saleId, ItemId = itemId };

            _saleRepositoryMock
                .Setup(x => x.GetByIdAsync(saleId))
                .ReturnsAsync((Sale)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("sale not found", ex.Message);
        }

        [Fact]
        public async Task Handle_WhenSaleIsCancelled_ShouldThrowException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var command = new CancelSaleItemCommand { SaleId = saleId, ItemId = itemId };

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
            var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("it is not possible to cancel items from a sale that has already been canceled.", ex.Message);
        }
    }
}