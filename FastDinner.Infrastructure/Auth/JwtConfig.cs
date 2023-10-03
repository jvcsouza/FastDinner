namespace FastDinner.Infrastructure.Auth;

public class JwtConfig
{
    public static string SectionName => "JwtConfig";
    public string Secret { get; init; } = null!;
    public int ExpirationInMinutes { get; init; }
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
}