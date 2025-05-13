using System;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sale.CancelSale
{
    public class CancelSaleCommand : IRequest<CancelSaleResult>
    {
        public Guid Id { get; set; }
    }
}