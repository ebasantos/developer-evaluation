namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale
{
    public class CancelSaleRequest
    {
        public CancelSaleRequest()
        {

        }
        public CancelSaleRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}