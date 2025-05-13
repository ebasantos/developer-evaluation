using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Sale.CreateSale;
using MassTransit;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IBus _bus;

        public CreateSaleCommandHandler(ISaleRepository saleRepository, IBus bus)
        {
            _saleRepository = saleRepository;
            _bus = bus;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = new Ambev.DeveloperEvaluation.Domain.Entities.Sale(
                request.SaleNumber,
                request.SaleDate,
                request.CustomerId,
                request.CustomerName,
                request.BranchId,
                request.BranchName
            );

            foreach (var item in request.Items)
            {
                sale.AddItem(
                    item.ProductId,
                    item.ProductName,
                    item.Quantity,
                    item.UnitPrice
                );
            }

            await _saleRepository.AddAsync(sale);

            if (sale.HasEvent)
            {
                foreach (var @event in sale._domainEvents)
                    await _bus.Publish(@event);
            }

            return new CreateSaleResult(sale.Id);
        }
    }
}