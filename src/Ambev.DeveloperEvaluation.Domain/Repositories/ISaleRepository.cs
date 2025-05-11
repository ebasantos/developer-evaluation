using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> GetByIdAsync(Guid id);
        Task<IEnumerable<Sale>> GetAllAsync();
        Task<Sale> GetBySaleNumberAsync(string saleNumber);
        Task AddAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(Guid id);
    }
} 