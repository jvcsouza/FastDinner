using FastDinner.Application.Commands.Menu;
using FastDinner.Application.Common;
using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Contracts.Menu;
using FastDinner.Domain.Model;
using MediatR;

namespace FastDinner.Application.Handlers;

public class MenuCommandHandler :
    IRequestHandler<CreateMenuCommand, MenuResponse>,
    IRequestHandler<UpdateMenuCommand, MenuResponse>,
    IRequestHandler<AddCategoryToMenuCommand, MenuDetailResponse>,
    IRequestHandler<AddMenuItemToCategoryCommand, MenuDetailResponse>
{
    // private readonly IRestaurantRepository _restaurantRepository;
    // private readonly ICurrentUserService _currentUserService;
    // private readonly IMapper _mapper;
    private readonly IMenuRepository _menuRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantRepository _restaurantRepository;

    public MenuCommandHandler(IMenuRepository menuRepository, IProductRepository productRepository,
        IRestaurantRepository restaurantRepository, IUnitOfWork unitOfWork)
    {
        _menuRepository = menuRepository;
        _restaurantRepository = restaurantRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    private async Task<Menu> GetMenuAsync(Guid id)
    {
        var menu = await _menuRepository.GetByIdAsync(id);

        if (menu == null)
            throw new ApplicationException($"Menu with id {id} not found");

        return menu;
    }

    public async Task<MenuResponse> Handle(CreateMenuCommand command, CancellationToken cancellationToken)
    {
        var restaurantId = AppScope.Restaurant.ResturantId;

        var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);

        if (restaurant == null)
            throw new ApplicationException($"Restaurant with id {restaurantId} not found");

        var urlImage = command.Image?.ToString();

        var newMenu = restaurant.IncludeMenu(command.Name, command.Description, urlImage);

        await _unitOfWork.CommitAsync();

        return new MenuResponse(newMenu.Id, newMenu.Name, newMenu.Description, newMenu.Image);
    }

    public async Task<MenuResponse> Handle(UpdateMenuCommand command, CancellationToken cancellationToken)
    {
        var menu = await GetMenuAsync(command.Id);

        // Throw new NotFoundException(nameof(Menu), command.Id);

        var urlImage = command.Image.ToString();

        menu.Update(command.Name, command.Description, urlImage);

        // await _menuRepository.Update(menu);

        await _unitOfWork.CommitAsync();

        return new MenuResponse(menu.Id, menu.Name, menu.Description, menu.Image);
    }

    public async Task<MenuDetailResponse> Handle(AddCategoryToMenuCommand command, CancellationToken cancellationToken)
    {
        await _unitOfWork.ExecuteTransaction(async () =>
        {
            var menu = await GetMenuAsync(command.MenuId);

            menu.AddCategory(new MenuCategory(command.CategoryName, command.Description));

            await _menuRepository.UpdateAsync(menu);

        });

        var menu = await GetMenuAsync(command.MenuId);

        return new MenuDetailResponse(
            menu.Id,
            menu.Name,
            menu.Description,
            menu.Image,
            menu.Categories.Select(x => new CategoryMenuResponse(x.Id, x.Name, x.Description,
                x.MenuItems.Select(e => new MenuItemResponse(e.Id, e.Name, e.Description, e.Price))))
        );
    }

    public async Task<MenuDetailResponse> Handle(AddMenuItemToCategoryCommand command, CancellationToken cancellationToken)
    {
        var menu = await GetMenuAsync(command.MenuId);

        var category = menu.Categories.FirstOrDefault(x => x.Id == command.CategoryId);

        if (category == null)
            throw new ApplicationException($"Category with id {command.CategoryId} not found");

        var product = await _productRepository.GetByIdAsync(command.ProductId);

        category.AddItem(new MenuItem(product, command.ProductDescription, command.Price));

        // await _menuRepository.Update(menu);

        await _unitOfWork.CommitAsync();

        return new MenuDetailResponse(
            menu.Id,
            menu.Name,
            menu.Description,
            menu.Image,
            menu.Categories.Select(x => new CategoryMenuResponse(x.Id, x.Name, x.Description,
                x.MenuItems.Select(x => new MenuItemResponse(x.Id, x.Name, x.Description, x.Price))))
        );
    }
}