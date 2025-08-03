using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Restaurant;
using MediatR;

namespace FastDinner.Application.Queries;

public record RestaurantQuery : IQuery<IEnumerable<RestaurantResponse>>;