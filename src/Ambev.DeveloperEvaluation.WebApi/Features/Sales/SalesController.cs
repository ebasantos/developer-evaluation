using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Commands;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISaleRepository _saleRepository;

        public SalesController(IMediator mediator, ISaleRepository saleRepository)
        {
            _mediator = mediator;
            _saleRepository = saleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _saleRepository.GetAllAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
                return NotFound();
            return Ok(sale);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSaleCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleCommand command)
        {
            if (id != command.Id)
                return BadRequest();
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _mediator.Send(new CancelSaleCommand { Id = id });
            return NoContent();
        }

        [HttpPost("{saleId}/items/{itemId}/cancel")]
        public async Task<IActionResult> CancelItem(Guid saleId, Guid itemId)
        {
            await _mediator.Send(new CancelSaleItemCommand { SaleId = saleId, ItemId = itemId });
            return NoContent();
        }
    }
} 