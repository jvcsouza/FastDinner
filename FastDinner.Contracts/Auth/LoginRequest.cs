namespace FastDinner.Contracts.Auth;

public record LoginRequest(
    string Username,
    string Password);