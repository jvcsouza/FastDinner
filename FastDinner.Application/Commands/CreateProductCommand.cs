using FastDinner.Contracts.Product;
using MediatR;

namespace FastDinner.Application.Commands;

public record CreateProductCommand (
    string Name
) : IRequest<ProductResponse>;