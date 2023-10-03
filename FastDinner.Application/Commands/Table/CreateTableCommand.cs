using FastDinner.Contracts.Table;
using MediatR;

namespace FastDinner.Application.Commands.Table;

public record CreateTableCommand (
    string Description,
    int Seats
) : IRequest<TableResponse>;