using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Commands;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Handlers
{
    public class UpdateSaleCommandHandler : IRequestHandler<UpdateSaleCommand>
    {
        private readonly ISaleRepository _saleRepository;

        public UpdateSaleCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.Id);
            if (sale == null)
                throw new Exception("Venda não encontrada.");

            if (sale.IsCancelled)
                throw new Exception("Não é possível atualizar uma venda cancelada.");

            // Atualizar itens
            foreach (var item in request.Items)
            {
                sale.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
            }

            await _saleRepository.UpdateAsync(sale);
        }
    }
} 