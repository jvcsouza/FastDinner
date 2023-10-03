using FastDinner.Contracts.Menu;
using MediatR;

namespace FastDinner.Application.Queries;

public record MenuQuery() : IRequest<IEnumerable<MenuResponse>>;
public record MenuCategoriesQuery(Guid menuId) : IRequest<IEnumerable<MenuCategoriesResponse>>;