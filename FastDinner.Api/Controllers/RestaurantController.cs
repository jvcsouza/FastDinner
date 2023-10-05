using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FastDinner.Application.Commands;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Restaurant;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace FastDinner.Api.Controllers
{
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
    public class RestaurantController : ApiController
    {
        private readonly ISender _mediator;

        public RestaurantController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var restaurants = await _mediator.Send<IEnumerable<RestaurantResponse>>(new RestaurantQuery());

            return Ok(restaurants);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRestaurantRequest request)
        {
            var restaurant = await _mediator.Send<RestaurantResponse>(new CreateRestaurantCommand(
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

            var restaurant = await _mediator.Send<RestaurantResponse>(new UpdateRestaurantCommand(
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