using FastDinner.Application.Commands.Table;
using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Table;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FastDinner.Api.Controllers
{
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
    public class TableController : ApiController
    {
        public TableController(ISender mediator, IUnitOfWork unitOfWork) 
            : base(mediator, unitOfWork) { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tables = await SendQueryAsync<IEnumerable<TableResponse>>(new TableQuery());

            return Ok(tables);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var table = await SendQueryAsync<TableResponse>(new TableQueryById(id));

            return Ok(table);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTableRequest request)
        {
            var product = await SendCommandAsync(new CreateTableCommand(request.Description, request.Seats));

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }
    }
}
