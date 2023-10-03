using FastDinner.Application.Common.Interfaces.Auth;
using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Auth;
using MediatR;

namespace FastDinner.Application.Handlers;

public class AuthQueryHandler : IRequestHandler<LoginQuery, LoginResponse>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthQueryHandler(IEmployeeRepository employeeRepository, ITokenGenerator tokenGenerator)
    {
        _employeeRepository = employeeRepository;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<LoginResponse> Handle(LoginQuery command, CancellationToken cancellationToken)
    {
        if (await _employeeRepository.GetByEmail(command.Email) is not { } user)
            throw new ApplicationException("Invalid email or password");

        if (user.Password != command.Password)
            throw new ApplicationException("Invalid email or password");

        var (token, expirationDate) = _tokenGenerator.GenerateToken(new Dictionary<string, object>()
        {
            {"id", user.Id},
            {"name", user.Name},
            {"email", user.Email}
        });

        var userResponse = new LoginUserResponse(user.Id, user.Name, user.Email);

        return new LoginResponse(userResponse, token, expirationDate);
    }
}