using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastDinner.Application.Common.Interfaces.Auth;
using FastDinner.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FastDinner.Infrastructure.Auth;

public class JwtTokenGenerator : ITokenGenerator
{
    private readonly JwtConfig _config;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtTokenGenerator(IOptions<JwtConfig> jwtConfig, IDateTimeProvider dateTimeProvider)
    {
        _config = jwtConfig.Value;
        _dateTimeProvider = dateTimeProvider;
    }

    public (string, DateTime) GenerateToken(Dictionary<string, object> payload)
    {
        var siginingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret)), SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            // new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            // new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            // new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var claim in claims)
        {
            payload.Add(claim.Type, claim.Value);
        }

        var expirationDate = _dateTimeProvider.UtcNow.AddMinutes(_config.ExpirationInMinutes);

        var securityToken = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            expires: expirationDate,
            claims: claims,
            signingCredentials: siginingCredentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(securityToken), expirationDate);
    }
}