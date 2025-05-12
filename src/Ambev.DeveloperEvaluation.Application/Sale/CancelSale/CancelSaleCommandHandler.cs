using Ambev.DeveloperEvaluation.Application.Sale.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MassTransit;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IBus _bus;

        public CancelSaleCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.Id) ?? throw new Exception("sale not found");

            sale.Cancel();

            await _saleRepository.UpdateAsync(sale);

            if (sale.HasEvent())
            {
                foreach (var @event in sale._domainEvents)
                    await _bus.Publish(@event)
            }

            return new CancelSaleResult(request.Id);
        }
    }
}