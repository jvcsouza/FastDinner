using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastDinner.Application.Commands;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Menu;
using FastDinner.Contracts.Restaurant;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastDinner.Api.Controllers
{
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

            return Ok(restaurant);
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> Put(Guid id, [FromBody] UpdateMenuRequest request)
        // {
        //     if (id != request.Id)
        //         return BadRequest();

        //     var menu = await _mediator.Send<MenuResponse>(new UpdateMenuCommand(
        //         request.Id,
        //         request.Name,
        //         request.Description,
        //         request.Image
        //     ));

        //     return Ok(menu);
        // }
    }
}