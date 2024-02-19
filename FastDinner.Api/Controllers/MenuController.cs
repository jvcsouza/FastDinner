using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FastDinner.Application.Commands.Menu;
using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Menu;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastDinner.Api.Controllers
{
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
    public class MenuController : ApiController
    {
        public MenuController(ISender mediator, IUnitOfWork unitOfWork) 
            : base(mediator, unitOfWork) { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var menus = await SendQueryAsync<IEnumerable<MenuResponse>>(new MenuQuery());

            return Ok(menus);
        }

        [HttpGet("{menuId:guid}")]
        public async Task<IActionResult> Get(Guid menuId)
        {
            // TODO: investigar o porquê desse código não funcionar, appscope do DbContext nulo
            //var menuRepository = DependencyResolver.Get<IMenuRepository>();
            //var menu = await menuRepository.GetByIdAsync(menuId);

            var menus = await SendQueryAsync<MenuDetailResponse>(new MenuQueryById(menuId));

            return Ok(menus);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateMenuRequest request)
        {
            var menu = await SendCommandAsync(new CreateMenuCommand(
                request.Name,
                request.Description,
                request.Image
            ));

            return CreatedAtAction(nameof(Get), new { id = menu.Id }, menu);
        }

        [HttpPut("{menuId:guid}")]
        public async Task<IActionResult> Put(Guid menuId, [FromBody] UpdateMenuRequest request)
        {
            if (menuId != request.Id)
                return BadRequest();

            var menu = await SendCommandAsync(new UpdateMenuCommand(
                request.Id,
                request.Name,
                request.Description,
                request.Image
            ));

            return Ok(menu);
        }

        [HttpPost("{menuId:guid}/category")]
        public async Task<IActionResult> PostCategory(Guid menuId, [FromBody] AddCategoryMenuRequest request)
        {
            if (menuId != request.MenuId)
                return BadRequest();

            var menu = await SendCommandAsync(new AddCategoryToMenuCommand(
                request.MenuId,
                request.Name,
                request.Description
            ));

            return Ok(menu);
        }

        [HttpGet("{menuId:guid}/category")]
        public async Task<IActionResult> GetCategories(Guid menuId)
        {
            if (menuId == Guid.NewGuid())
                return BadRequest();

            var menuCategories = await SendQueryAsync<IEnumerable<MenuCategoriesResponse>>(new MenuCategoriesQuery(menuId));

            return Ok(menuCategories);
        }

        [HttpPatch("{menuId:guid}/category/{categoryId:guid}/item")]
        public async Task<IActionResult> PatchCategoryItem(Guid menuId, Guid categoryId, [FromBody] AddItemToCategoryMenuRequest request)
        {
            if (menuId != request.MenuId)
                return BadRequest();

            if (categoryId != request.CategoryId)
                return BadRequest();

            var menu = await SendCommandAsync(new AddMenuItemToCategoryCommand(
                request.MenuId,
                request.CategoryId,
                request.ProductId,
                request.ProductDescription,
                request.Price
            ));

            return Ok(menu);
        }
    }
}