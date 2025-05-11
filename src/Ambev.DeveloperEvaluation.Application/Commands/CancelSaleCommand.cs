using System;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands
{
    public class CancelSaleCommand : IRequest
    {
        public Guid Id { get; set; }
    }
} 