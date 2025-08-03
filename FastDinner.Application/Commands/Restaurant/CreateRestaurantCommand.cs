using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Restaurant;
using MediatR;

namespace FastDinner.Application.Commands.Restaurant;

public record CreateRestaurantCommand(
    string Name,
    string Address,
    string Phone,
    string Email
) : ICommand<RestaurantResponse>;