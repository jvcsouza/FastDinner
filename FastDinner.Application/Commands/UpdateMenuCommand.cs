using FastDinner.Contracts.Menu;
using MediatR;

namespace FastDinner.Application.Commands;

public record UpdateMenuCommand (
    Guid Id, 
    string Name,
    string Description,
    byte[] Image
) : IRequest<MenuResponse>;