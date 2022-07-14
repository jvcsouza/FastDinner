using FastDinner.Application.Commands;
using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Contracts.Menu;
using FastDinner.Contracts.Restaurant;
using FastDinner.Domain.Model;
using MediatR;

namespace FastDinner.Application.Handlers;

public class RestaurantCommandHandler
    : IRequestHandler<CreateRestaurantCommand, RestaurantResponse>
{
    // private readonly IRestaurantRepository _restaurantRepository;
    // private readonly ICurrentUserService _currentUserService;
    // private readonly IMapper _mapper;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RestaurantCommandHandler(IRestaurantRepository restaurantRepository, IUnitOfWork unitOfWork)
    {
        _restaurantRepository = restaurantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RestaurantResponse> Handle(CreateRestaurantCommand command, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.Create(new Restaurant(
            command.Name,
            command.Address,
            command.Phone,
            command.Email
        ));

        await _unitOfWork.CommitAsync();

        return new RestaurantResponse(restaurant.Id, restaurant.Name,
            restaurant.Address, restaurant.Phone, restaurant.Email);
    }

    // public async Task<MenuResponse> Handle(UpdateMenuCommand command, CancellationToken cancellationToken)
    // {
    //     var menu = await _menuRepository.GetById(command.Id);

    //     // Throw new NotFoundException(nameof(Menu), command.Id);

    //     var urlImage = command.Image.ToString();

    //     menu.Update(command.Name, command.Description, urlImage);

    //     await _menuRepository.Update(menu);

    //     await _unitOfWork.CommitAsync();

    //     return new MenuResponse(menu.Id, menu.Name, menu.Description, menu.Image);
    // }
}