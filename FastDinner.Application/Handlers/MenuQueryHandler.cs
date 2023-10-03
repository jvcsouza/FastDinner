using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Menu;
using MediatR;

namespace FastDinner.Application.Handlers;

public class MenuQueryHandler :
    IRequestHandler<MenuQuery, IEnumerable<MenuResponse>>,
    IRequestHandler<MenuCategoriesQuery, IEnumerable<MenuCategoriesResponse>>
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
        var menuCategories = await _menuRepository.GetAllCategories(request.menuId);

        return menuCategories.Select(x => new MenuCategoriesResponse(x.Id, x.Name));
    }
}