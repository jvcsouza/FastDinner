using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Product;

namespace FastDinner.Application.Commands.Product;

public record CreateProductCommand(
    string Name
) : ICommand<ProductResponse>;