using System;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sale.CancelSaleItem
{
    public class CancelSaleItemCommand : IRequest<CancelSaleItemResult>
    {
        public Guid SaleId { get; set; }
        public Guid ItemId { get; set; }
    }
}