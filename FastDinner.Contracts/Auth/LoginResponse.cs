namespace FastDinner.Contracts.Auth;

public record LoginResponse(
    LoginUserResponse User,
    string Token,
    DateTime expirationDate);

public record LoginUserResponse(
    Guid Id,
    string Name,
    string Email);