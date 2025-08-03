using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Menu;

namespace FastDinner.Application.Commands.Menu;

public record AddCategoryToMenuCommand(
    Guid MenuId,
    string CategoryName,
    string Description
    // List<CategoryMenuProduct> CategoryMenuProducts
) : ICommand<MenuDetailResponse>;

public record CategoryMenuProduct(
    Guid ProductId,
    string ProcuctName,
    string ProductDescription,
    decimal Price
);