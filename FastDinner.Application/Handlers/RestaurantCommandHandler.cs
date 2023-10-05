using FastDinner.Application.Commands;
using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Contracts.Restaurant;
using FastDinner.Domain.Model;
using MediatR;

namespace FastDinner.Application.Handlers;

public class RestaurantCommandHandler :
    IRequestHandler<CreateRestaurantCommand, RestaurantResponse>,
    IRequestHandler<UpdateRestaurantCommand, RestaurantResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RestaurantCommandHandler(IRestaurantRepository restaurantRepository, IUnitOfWork unitOfWork)
    {
        _restaurantRepository = restaurantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RestaurantResponse> Handle(CreateRestaurantCommand command, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.CreateAsync(new Restaurant(
            command.Name,
            command.Address,
            command.Phone,
            command.Email
        ));

        await _unitOfWork.CommitAsync();

        return new RestaurantResponse(restaurant.Id, restaurant.Name,
            restaurant.Address, restaurant.Phone, restaurant.Email);
    }

    public async Task<RestaurantResponse> Handle(UpdateRestaurantCommand command, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(command.Id);

        // Throw new NotFoundException(nameof(Menu), command.Id);
        restaurant.Update(command.Name, command.Address, command.Phone, command.Email);

        await _restaurantRepository.UpdateAsync(restaurant);

        _unitOfWork.AddEventAfterSave(InsertOf, new[] { 1 });

        await _unitOfWork.CommitAsync();

        return new RestaurantResponse(restaurant.Id, restaurant.Name,
            restaurant.Address, restaurant.Phone, restaurant.Email);
    }

    private Task InsertOf(object[] arg)
    {
        _unitOfWork.AddEventAfterSave(InsertOfTwo, arg);

        return Task.CompletedTask;
    }

    private Task InsertOfTwo(object[] arg)
    {
        return Task.CompletedTask;
    }
}