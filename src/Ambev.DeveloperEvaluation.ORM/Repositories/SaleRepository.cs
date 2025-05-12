using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Sale> AddAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task DeleteAsync(Sale sale)
        {
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            // i believe it should  only look for active sales...
            return await _context.Sales.Where(x => !x.IsCancelled).ToListAsync();
        }

        public async Task<Sale> GetByIdAsync(Guid id)
        {
            return await _context.Sales.FindAsync(id);
        }

        public async Task<Sale> GetBySaleNumberAsync(string saleNumber)
        {
            return await _context.Sales.FirstOrDefaultAsync(x => x.SaleNumber == saleNumber);
        }

        public async Task<Sale> UpdateAsync(Sale sale)
        {
            var updateTask = Task.Run(() => _context.Sales.Update(sale));
            await updateTask;
            await _context.SaveChangesAsync();
            return sale;
        }
    }
}
