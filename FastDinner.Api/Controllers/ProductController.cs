using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastDinner.Application.Commands;
using FastDinner.Application.Common;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastDinner.Api.Controllers
{
    public class ProductController : ApiController
    {
        private readonly ISender _mediator;

        public ProductController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _mediator.Send<IEnumerable<ProductResponse>>(new ProductQuery());

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductRequest request)
        {
            var product = await _mediator.Send<ProductResponse>(new CreateProductCommand(request.Name));

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateProductRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var product = await _mediator.Send<ProductResponse>(new UpdateProductCommand(
                request.Id,
                request.Name
            ));

            return Ok(product);
        }
    }
}