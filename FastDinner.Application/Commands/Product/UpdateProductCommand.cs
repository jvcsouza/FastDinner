using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Product;
using MediatR;

namespace FastDinner.Application.Commands.Product;

public record UpdateProductCommand(
    Guid Id,
    string Name
) : ICommand<ProductResponse>;