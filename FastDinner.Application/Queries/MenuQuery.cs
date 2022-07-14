using FastDinner.Contracts.Menu;
using MediatR;

namespace FastDinner.Application.Queries;

public record MenuQuery() : IRequest<IEnumerable<MenuResponse>>;