using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Table;
using MediatR;

namespace FastDinner.Application.Queries;

public record TableQuery : IQuery<IEnumerable<TableResponse>>;
public record TableQueryById(Guid TableId) : IQuery<TableResponse>;
