using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Restaurant;

namespace FastDinner.Application.Queries;

public record RestaurantQuery : IQuery<IEnumerable<RestaurantResponse>>;