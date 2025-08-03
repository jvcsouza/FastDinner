using FastDinner.Application.Common.Interfaces;
using FastDinner.Contracts.Auth;

namespace FastDinner.Application.Queries;

public record LoginQuery(
    string Email,
    string Password
) : IQuery<LoginResponse>;