namespace Ambev.DeveloperEvaluation.Application.Sale.CreateSale
{
    public class CreateSaleResult
    {
        public CreateSaleResult()
        {

        }
        public CreateSaleResult(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

    }
}
