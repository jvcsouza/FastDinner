using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Table;

namespace FastDinner.Application.Commands.Table;

public record CreateTableCommand (
    string Description,
    int Seats
) : ICommand<TableResponse>;