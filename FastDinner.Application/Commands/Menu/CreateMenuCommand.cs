using FastDinner.Contracts.Menu;
using MediatR;

namespace FastDinner.Application.Commands.Menu;

public record CreateMenuCommand (
    string Name,
    string Description,
    string Image
) : IRequest<MenuResponse>;