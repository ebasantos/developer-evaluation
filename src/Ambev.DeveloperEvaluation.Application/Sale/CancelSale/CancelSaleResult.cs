namespace Ambev.DeveloperEvaluation.Application.Sale.CancelSale
{
    public class CancelSaleResult
    {
        public CancelSaleResult()
        {

        }
        public CancelSaleResult(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
