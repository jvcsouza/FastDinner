using FastDinner.Contracts.Product;
using MediatR;

namespace FastDinner.Application.Commands;

public record UpdateProductCommand (
    Guid Id,
    string Name
) : IRequest<ProductResponse>;