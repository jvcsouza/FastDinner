using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Menu;
using MediatR;

namespace FastDinner.Application.Commands.Menu;

public record AddMenuItemToCategoryCommand(
    Guid MenuId,
    Guid CategoryId,
    Guid ProductId,
    string ProductName,
    string ProductDescription,
    decimal Price
) : ICommand<MenuDetailResponse>;