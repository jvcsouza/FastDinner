using FastDinner.Contracts.Product;
using MediatR;

namespace FastDinner.Application.Queries;

public record ProductQuery() : IRequest<IEnumerable<ProductResponse>>;