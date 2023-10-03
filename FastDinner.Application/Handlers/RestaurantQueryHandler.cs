using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Restaurant;
using MediatR;

namespace FastDinner.Application.Handlers;

public class RestaurantQueryHandler
    : IRequestHandler<RestaurantQuery, IEnumerable<RestaurantResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public RestaurantQueryHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<IEnumerable<RestaurantResponse>> Handle(RestaurantQuery request, CancellationToken cancellationToken)
    {
        var restaurants = await _restaurantRepository.GetAllAsync();

        return restaurants.Select(x => new RestaurantResponse(x.Id, x.Name, x.Address, x.Phone, x.Email));
    }
}