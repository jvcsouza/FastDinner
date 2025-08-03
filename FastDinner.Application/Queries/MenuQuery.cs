using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Menu;
using MediatR;

namespace FastDinner.Application.Queries;

public record MenuQuery : IQuery<IEnumerable<MenuResponse>>;
public record MenuQueryById(Guid MenuId) : IQuery<MenuDetailResponse>;
public record MenuCategoriesQuery(Guid MenuId) : IQuery<IEnumerable<MenuCategoriesResponse>>;