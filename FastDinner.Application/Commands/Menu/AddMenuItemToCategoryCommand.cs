using FastDinner.Contracts.Menu;
using MediatR;

namespace FastDinner.Application.Commands.Menu;

public record AddMenuItemToCategoryCommand(
    Guid MenuId,
    Guid CategoryId,
    Guid ProductId,
    string ProductDescription,
    decimal Price
) : IRequest<MenuDetailResponse>;