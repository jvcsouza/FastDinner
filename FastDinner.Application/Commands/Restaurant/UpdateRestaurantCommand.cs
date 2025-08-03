using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Restaurant;
using MediatR;

namespace FastDinner.Application.Commands.Restaurant;

public record UpdateRestaurantCommand(
    Guid Id,
    string Name,
    string Address,
    string Phone,
    string Email
) : ICommand<RestaurantResponse>;