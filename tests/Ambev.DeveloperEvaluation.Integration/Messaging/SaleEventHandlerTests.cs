using System;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sale.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sale.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sale.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Messaging
{
    public class SaleEventHandlerTests : IAsyncDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly ITestHarness _testHarness;

        public SaleEventHandlerTests()
        {
            var services = new ServiceCollection();

            services.AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<CreateSaleConsumer>();
                cfg.AddConsumer<CancelSaleConsumer>();
                cfg.AddConsumer<CancelSaleItemConsumer>();
            });

            _serviceProvider = services.BuildServiceProvider();
            _testHarness = _serviceProvider.GetRequiredService<ITestHarness>();
            _testHarness.Start();
        }

        [Fact]
        public async Task CreateSaleEvent_ShouldBePublishedAndConsumed()
        {
            // Arrange
            var sale = new Sale(
                "SALE-001",
                DateTime.UtcNow,
                Guid.NewGuid(),
                "John Doe",
                Guid.NewGuid(),
                "Branch 1"
            );
            var @event = new SaleCreatedEvent(sale);

            // Act
            await _testHarness.Bus.Publish(@event);

            // Assert
            Assert.True(await _testHarness.Published.Any<SaleCreatedEvent>());
            Assert.True(await _testHarness.Consumed.Any<SaleCreatedEvent>());
        }

        [Fact]
        public async Task CancelSaleEvent_ShouldBePublishedAndConsumed()
        {
            // Arrange
            var sale = new Sale(
                "SALE-002",
                DateTime.UtcNow,
                Guid.NewGuid(),
                "Jane Doe",
                Guid.NewGuid(),
                "Branch 2"
            );
            var @event = new SaleCancelledEvent(sale);

            // Act
            await _testHarness.Bus.Publish(@event);

            // Assert
            Assert.True(await _testHarness.Published.Any<SaleCancelledEvent>());
            Assert.True(await _testHarness.Consumed.Any<SaleCancelledEvent>());
        }

        [Fact]
        public async Task CancelSaleItemEvent_ShouldBePublishedAndConsumed()
        {
            // Arrange
            var sale = new Sale(
                "SALE-003",
                DateTime.UtcNow,
                Guid.NewGuid(),
                "Bob Smith",
                Guid.NewGuid(),
                "Branch 3"
            );
            var productId = Guid.NewGuid();
            sale.AddItem(productId, "Product 1", 5, 100m);
            var @event = new ItemCancelledEvent(sale, productId);

            // Act
            await _testHarness.Bus.Publish(@event);

            // Assert
            Assert.True(await _testHarness.Published.Any<ItemCancelledEvent>());
            Assert.True(await _testHarness.Consumed.Any<ItemCancelledEvent>());
        }

        [Fact]
        public async Task MultipleEvents_ShouldBeHandledInOrder()
        {
            // Arrange
            var sale = new Sale(
                "SALE-004",
                DateTime.UtcNow,
                Guid.NewGuid(),
                "Alice Johnson",
                Guid.NewGuid(),
                "Branch 4"
            );

            var createEvent = new SaleCreatedEvent(sale);
            var productId = Guid.NewGuid();
            sale.AddItem(productId, "Product 2", 3, 200m);
            var cancelItemEvent = new ItemCancelledEvent(sale, productId);
            var cancelSaleEvent = new SaleCancelledEvent(sale);

            // Act
            await _testHarness.Bus.Publish(createEvent);
            await _testHarness.Bus.Publish(cancelItemEvent);
            await _testHarness.Bus.Publish(cancelSaleEvent);

            // Assert
            Assert.True(await _testHarness.Published.Any<SaleCreatedEvent>());
            Assert.True(await _testHarness.Published.Any<ItemCancelledEvent>());
            Assert.True(await _testHarness.Published.Any<SaleCancelledEvent>());

            Assert.True(await _testHarness.Consumed.Any<SaleCreatedEvent>());
            Assert.True(await _testHarness.Consumed.Any<ItemCancelledEvent>());
            Assert.True(await _testHarness.Consumed.Any<SaleCancelledEvent>());
        }

        public async ValueTask DisposeAsync()
        {
            await _testHarness.Stop();
            await _serviceProvider.DisposeAsync();
        }
    }
} 