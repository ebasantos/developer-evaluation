using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleById
{
    public class GetSaleByIdRequestValidator : AbstractValidator<GetSaleByIdRequest>
    {
        public GetSaleByIdRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
