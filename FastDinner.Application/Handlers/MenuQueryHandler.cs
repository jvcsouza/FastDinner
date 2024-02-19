using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Menu;
using MediatR;

namespace FastDinner.Application.Handlers;

public class MenuQueryHandler :
    IRequestHandler<MenuQuery, IEnumerable<MenuResponse>>,
    IRequestHandler<MenuCategoriesQuery, IEnumerable<MenuCategoriesResponse>>,
    IRequestHandler<MenuQueryById, MenuDetailResponse>
{
    private readonly IMenuRepository _menuRepository;

    public MenuQueryHandler(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<IEnumerable<MenuResponse>> Handle(MenuQuery _, CancellationToken cancellationToken)
    {
        var menus = await _menuRepository.GetAllAsync();

        return menus.Select(x => new MenuResponse(x.Id, x.Name, x.Description, x.Image));
    }

    public async Task<IEnumerable<MenuCategoriesResponse>> Handle(MenuCategoriesQuery request, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByIdAsync(request.MenuId);

        return menu.Categories.Select(x => new MenuCategoriesResponse(x.Id, x.Name, x.Description, x.MenuItems.Count));
    }

    public async Task<MenuDetailResponse> Handle(MenuQueryById request, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByIdAsync(request.MenuId);

        if (menu is null)
            throw new ApplicationException($"Menu {request.MenuId} not found!");

        return new MenuDetailResponse(menu.Id, menu.Name, menu.Description, menu.Image, menu.Main,
            menu.Categories.Select(x => new CategoryMenuResponse(x.Id, x.Name, x.Description,
                x.MenuItems.Select(e => new MenuItemResponse(e.Id, e.Name, e.Description, e.Price)))));
    }
}