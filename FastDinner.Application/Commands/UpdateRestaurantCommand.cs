using FastDinner.Contracts.Restaurant;
using MediatR;

namespace FastDinner.Application.Commands;

public record UpdateRestaurantCommand(
    Guid Id,
    string Name,
    string Address,
    string Phone,
    string Email
) : IRequest<RestaurantResponse>;