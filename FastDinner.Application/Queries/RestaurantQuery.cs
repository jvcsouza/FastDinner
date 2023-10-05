using FastDinner.Contracts.Restaurant;
using MediatR;

namespace FastDinner.Application.Queries;

public record RestaurantQuery : IRequest<IEnumerable<RestaurantResponse>>;