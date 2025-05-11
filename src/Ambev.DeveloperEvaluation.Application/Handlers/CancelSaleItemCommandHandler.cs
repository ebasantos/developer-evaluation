using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Commands;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Handlers
{
    public class CancelSaleItemCommandHandler : IRequestHandler<CancelSaleItemCommand>
    {
        private readonly ISaleRepository _saleRepository;

        public CancelSaleItemCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.SaleId);
            if (sale == null)
                throw new Exception("Venda não encontrada.");

            if (sale.IsCancelled)
                throw new Exception("Não é possível cancelar itens de uma venda já cancelada.");

            sale.CancelItem(request.ItemId);
            await _saleRepository.UpdateAsync(sale);
        }
    }
} 