using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Product;

namespace FastDinner.Application.Commands.Product;

public record UpdateProductCommand(
    Guid Id,
    string Name
) : ICommand<ProductResponse>;