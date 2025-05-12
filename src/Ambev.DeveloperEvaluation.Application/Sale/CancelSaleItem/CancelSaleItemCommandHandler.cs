using Ambev.DeveloperEvaluation.Application.Sale.CancelSaleItem;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    public class CancelSaleItemCommandHandler : IRequestHandler<CancelSaleItemCommand, CancelSaleItemResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public CancelSaleItemCommandHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _mapper = mapper;
            _saleRepository = saleRepository;
        }

        public async Task Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.SaleId) ?? throw new Exception("sale not found");

            if (sale.IsCancelled)
                throw new Exception("it is not possible to cancel items from a sale that has already been canceled.");

            sale.CancelItem(request.ItemId);
            await _saleRepository.UpdateAsync(sale);

            return _mapper<CancelSaleItemResult>(request);
        }
    }
}