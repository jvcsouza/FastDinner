using FastDinner.Application.Commands.Table;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Table;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastDinner.Api.Controllers
{
    public class TableController : ApiController
    {
        private readonly ISender _mediator;

        public TableController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _mediator.Send<IEnumerable<TableResponse>>(new TableQuery());

            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var products = await _mediator.Send<IEnumerable<TableResponse>>(new TableQuery());

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTableRequest request)
        {
            var product = await _mediator.Send<TableResponse>(new CreateTableCommand(request.Description, request.Seats));

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }
    }
}
