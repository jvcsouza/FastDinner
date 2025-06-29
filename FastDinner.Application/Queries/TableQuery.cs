﻿using FastDinner.Contracts.Table;
using MediatR;

namespace FastDinner.Application.Queries;

public record TableQuery : IRequest<IEnumerable<TableResponse>>;
public record TableQueryById(Guid TableId) : IRequest<TableResponse>;
