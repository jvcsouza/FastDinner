using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FastDinner.Application.Commands.Product;
using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastDinner.Api.Controllers
{
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
    public class ProductController : ApiController
    {
        public ProductController(ISender mediator, IUnitOfWork unitOfWork) 
            : base(mediator, unitOfWork) { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await SendCommandAsync<IEnumerable<ProductResponse>>(new ProductQuery());

            return Ok(products);
        }

        [HttpGet("{productId:guid}")]
        public async Task<IActionResult> Get(Guid productId)
        {
            var products = await SendCommandAsync<ProductResponse>(new ProductQueryById(productId));

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductRequest request)
        {
            var product = await SendCommandAsync(new CreateProductCommand(request.Name));

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateProductRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var product = await SendCommandAsync(new UpdateProductCommand(
                request.Id,
                request.Name
            ));

            return Ok(product);
        }
    }
}