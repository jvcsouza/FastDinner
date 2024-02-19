using FastDinner.Contracts.Product;
using MediatR;

namespace FastDinner.Application.Queries;

public record ProductQuery : IRequest<IEnumerable<ProductResponse>>;
public record ProductQueryById(Guid ProductId) : IRequest<ProductResponse>;