using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FastDinner.Application.Commands.Restaurant;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Restaurant;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using FastDinner.Application.Common.Interfaces.Repositories;

namespace FastDinner.Api.Controllers
{
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
    public class RestaurantController : ApiController
    {
        public RestaurantController(ISender mediator, IUnitOfWork unitOfWork) 
            : base(mediator, unitOfWork) { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var restaurants = await SendQueryAsync<IEnumerable<RestaurantResponse>>(new RestaurantQuery());

            return Ok(restaurants);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRestaurantRequest request)
        {
            var restaurant = await SendCommandAsync(new CreateRestaurantCommand(
                request.Name,
                request.Address,
                request.Phone,
                request.Email
            ));

            return CreatedAtAction(nameof(Get), new { id = restaurant.Id }, restaurant);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateRestaurantRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var restaurant = await SendCommandAsync(new UpdateRestaurantCommand(
                request.Id,
                request.Name,
                request.Address,
                request.Phone,
                request.Email
            ));

            return Ok(restaurant);
        }
    }
}