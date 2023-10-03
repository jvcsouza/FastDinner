using FastDinner.Contracts.Menu;
using MediatR;

namespace FastDinner.Application.Commands.Menu;

public record AddCategoryToMenuCommand(
    Guid MenuId,
    string CategoryName,
    string Description
    // List<CategoryMenuProduct> CategoryMenuProducts
) : IRequest<MenuDetailResponse>;

public record CategoryMenuProduct(
    Guid ProductId,
    string ProcuctName,
    string ProductDescription,
    decimal Price
);