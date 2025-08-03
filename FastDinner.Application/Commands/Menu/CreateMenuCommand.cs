using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Menu;

namespace FastDinner.Application.Commands.Menu;

public record CreateMenuCommand (
    string Name,
    string Description,
    string Image
) : ICommand<MenuResponse>;