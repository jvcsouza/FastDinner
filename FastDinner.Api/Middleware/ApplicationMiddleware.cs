using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FastDinner.Api.Middleware;
public class ApplicationMiddleware
{
    private readonly RequestDelegate _next;

    public ApplicationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await CreateApplicationScope(context);
        await _next(context);
    }

    private async Task CreateApplicationScope(HttpContext context)
    {
        var restaurantId = Guid.Empty;
        if (context.Request.Headers.TryGetValue("X-Resturant-ID", out var restaurantIdHeader))
        {
            restaurantId = Guid.Parse(restaurantIdHeader);
        }

        var applicationScope = new Dictionary<string, object>
        {
            {"restaurantId", restaurantId}
        };
        
        context.Items.TryAdd("applicationScope", applicationScope);

        // Create application scope
        await Task.FromResult(true);
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        throw new NotImplementedException();
    }
}