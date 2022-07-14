using FastDinner.Contracts.Menu;
using MediatR;

namespace FastDinner.Application.Commands;

public record CreateMenuCommand (
    string Name,
    string Description,
    byte[] Image
) : IRequest<MenuResponse>;