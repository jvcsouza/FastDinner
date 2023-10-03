using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastDinner.Application.Commands.Menu;
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

            return CreatedAtAction(nameof(Get), new { id = menu.Id }, menu);
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

        [HttpPost("{menuId}/category")]
        public async Task<IActionResult> PostCategory(Guid menuId, [FromBody] AddCategoryMenuRequest request)
        {
            if (menuId != request.MenuId)
                return BadRequest();

            var menu = await _mediator.Send<MenuDetailResponse>(new AddCategoryToMenuCommand(
                request.MenuId,
                request.Name,
                request.Description
            ));

            return Ok(menu);
        }

        [HttpGet("{menuId}/category")]
        public async Task<IActionResult> GetCategories(Guid menuId)
        {
            if (menuId == Guid.NewGuid())
                return BadRequest();

            var menuCategories = await _mediator.Send<IEnumerable<MenuCategoriesResponse>>(new MenuCategoriesQuery(menuId));

            return Ok(menuCategories);
        }

        [HttpPatch("{menuId}/category/{categoryId}/item")]
        public async Task<IActionResult> PatchCategoryItem(Guid menuId, Guid categoryId, [FromBody] AddItemToCategoryMenuRequest request)
        {
            if (menuId != request.MenuId)
                return BadRequest();

            if (categoryId != request.CategoryId)
                return BadRequest();

            var menu = await _mediator.Send<MenuDetailResponse>(new AddMenuItemToCategoryCommand(
                request.MenuId,
                request.CategoryId,
                request.ProductId,
                request.ProductName,
                request.ProductDescription,
                request.Price
            ));

            return Ok(menu);
        }
    }
}