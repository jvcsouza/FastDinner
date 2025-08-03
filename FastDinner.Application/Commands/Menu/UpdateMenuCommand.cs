using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Menu;

namespace FastDinner.Application.Commands.Menu;

public record UpdateMenuCommand (
    Guid Id, 
    string Name,
    string Description,
    byte[] Image
) : ICommand<MenuResponse>;