using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Specifications
{
    public class CanCancelSaleItemSpecification : ISpecification<SaleItem>
    {
        public bool IsSatisfiedBy(SaleItem entity)
        {
            return !entity.IsCancelled;
        }
    }
}
