using FastDinner.Contracts.Product;
using MediatR;

namespace FastDinner.Application.Commands.Product;

public record CreateProductCommand(
    string Name
) : IRequest<ProductResponse>;