using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Table;
using MediatR;

namespace FastDinner.Application.Commands.Table;

public record CreateTableCommand (
    string Description,
    int Seats
) : ICommand<TableResponse>;