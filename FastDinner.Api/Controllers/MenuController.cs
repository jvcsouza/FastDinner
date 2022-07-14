using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastDinner.Application.Commands;
using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Common.Interfaces.Services;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Menu;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastDinner.Api.Controllers
{
    public class MenuController : ApiController
    {
        private readonly ISender _mediator;

        public MenuController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var menus = await _mediator.Send<IEnumerable<MenuResponse>>(new MenuQuery());

            return Ok(menus);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateMenuRequest request)
        {
            var menu = await _mediator.Send<MenuResponse>(new CreateMenuCommand(
                request.Name,
                request.Description,
                request.Image
            ));

            return Ok(menu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateMenuRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var menu = await _mediator.Send<MenuResponse>(new UpdateMenuCommand(
                request.Id,
                request.Name,
                request.Description,
                request.Image
            ));

            return Ok(menu);
        }
    }
}