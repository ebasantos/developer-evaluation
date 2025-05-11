using System;
using System.Collections.Generic;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands
{
    public class UpdateSaleCommand : IRequest
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public List<UpdateSaleItemCommand> Items { get; set; }
    }

    public class UpdateSaleItemCommand
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
} 