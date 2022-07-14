using FastDinner.Application.Commands;
using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Contracts.Menu;
using FastDinner.Domain.Model;
using MediatR;

namespace FastDinner.Application.Handlers;

public class MenuCommandHandler :
    IRequestHandler<CreateMenuCommand, MenuResponse>,
    IRequestHandler<UpdateMenuCommand, MenuResponse>
{
    // private readonly IRestaurantRepository _restaurantRepository;
    // private readonly ICurrentUserService _currentUserService;
    // private readonly IMapper _mapper;
    private readonly IMenuRepository _menuRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MenuCommandHandler(IMenuRepository menuRepository, IUnitOfWork unitOfWork)
    {
        _menuRepository = menuRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<MenuResponse> Handle(CreateMenuCommand command, CancellationToken cancellationToken)
    {
        var urlImage = command.Image?.ToString();

        var menu = await _menuRepository.Create(new Menu(
            command.Name,
            command.Description,
            urlImage
        ));

        await _unitOfWork.CommitAsync();

        return new MenuResponse(menu.Id, menu.Name, menu.Description, menu.Image);
    }

    public async Task<MenuResponse> Handle(UpdateMenuCommand command, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetById(command.Id);

        // Throw new NotFoundException(nameof(Menu), command.Id);

        var urlImage = command.Image.ToString();

        menu.Update(command.Name, command.Description, urlImage);

        // await _menuRepository.Update(menu);

        await _unitOfWork.CommitAsync();

        return new MenuResponse(menu.Id, menu.Name, menu.Description, menu.Image);
    }
}