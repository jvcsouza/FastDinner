using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FastDinner.Api.Middleware;

public class AuthenticationMiddleware
{
     private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
    }
}