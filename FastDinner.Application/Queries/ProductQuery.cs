using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Product;
using MediatR;

namespace FastDinner.Application.Queries;

public record ProductQuery : IQuery<IEnumerable<ProductResponse>>;
public record ProductQueryById(Guid ProductId) : IQuery<ProductResponse>;