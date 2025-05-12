using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Specifications
{
    public class CanCancellSaleSpecification : ISpecification<Sale>
    {
        public bool IsSatisfiedBy(Sale entity)
        {
            return !entity.IsCancelled;
        }
    }
}
