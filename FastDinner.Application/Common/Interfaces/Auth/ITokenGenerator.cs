using FastDinner.Domain.Model;

namespace FastDinner.Application.Common.Interfaces.Auth;

public interface ITokenGenerator
{
    (string, DateTime) GenerateToken(Dictionary<string, object> payload);
}