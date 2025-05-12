using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> GetByIdAsync(Guid id);
        Task<IEnumerable<Sale>> GetAllAsync();
        Task<Sale> GetBySaleNumberAsync(string saleNumber);
        Task<Sale> AddAsync(Sale sale);
        Task<Sale> UpdateAsync(Sale sale);
        Task DeleteAsync(Sale sale);
    }
}