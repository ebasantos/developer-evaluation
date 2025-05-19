using System;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Repositories
{
    public class SaleRepositoryTests : IDisposable
    {
        private readonly DefaultContext _context;
        private readonly SaleRepository _saleRepository;

        public SaleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DefaultContext(options);
            _saleRepository = new SaleRepository(_context);
        }

        [Fact]
        public async Task CreateSale_ShouldPersistSaleAndItems()
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

            sale.AddItem(Guid.NewGuid(), "Product 1", 5, 100m);
            sale.AddItem(Guid.NewGuid(), "Product 2", 3, 200m);

            // Act
            await _saleRepository.CreateAsync(sale);
            await _context.SaveChangesAsync();

            // Assert
            var persistedSale = await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == sale.Id);

            Assert.NotNull(persistedSale);
            Assert.Equal(2, persistedSale.Items.Count);
            Assert.Equal(900m, persistedSale.TotalAmount); // (5 * 100 * 0.9) + (3 * 200)
        }

        [Fact]
        public async Task GetById_WithExistingSale_ShouldReturnSaleWithItems()
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

            sale.AddItem(Guid.NewGuid(), "Product 3", 10, 150m);
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            // Act
            var result = await _saleRepository.GetByIdAsync(sale.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(1200m, result.TotalAmount); // 10 * 150 * 0.8 (20% discount)
        }

        [Fact]
        public async Task GetById_WithNonExistingSale_ShouldReturnNull()
        {
            // Act
            var result = await _saleRepository.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateSale_ShouldUpdateSaleAndItems()
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

            sale.AddItem(Guid.NewGuid(), "Product 4", 4, 100m);
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            // Act
            sale.AddItem(Guid.NewGuid(), "Product 5", 6, 200m);
            await _saleRepository.UpdateAsync(sale);
            await _context.SaveChangesAsync();

            // Assert
            var updatedSale = await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == sale.Id);

            Assert.NotNull(updatedSale);
            Assert.Equal(2, updatedSale.Items.Count);
            Assert.Equal(1480m, updatedSale.TotalAmount); // (4 * 100) + (6 * 200 * 0.9)
        }

        [Fact]
        public async Task DeleteSale_ShouldRemoveSaleAndItems()
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

            sale.AddItem(Guid.NewGuid(), "Product 6", 2, 300m);
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            // Act
            await _saleRepository.DeleteAsync(sale);
            await _context.SaveChangesAsync();

            // Assert
            var deletedSale = await _context.Sales.FindAsync(sale.Id);
            Assert.Null(deletedSale);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
} 