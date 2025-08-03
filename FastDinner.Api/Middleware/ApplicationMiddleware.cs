using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FastDinner.Application.Common.Interfaces.Services;

namespace FastDinner.Api.Middleware;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class ApplicationMiddleware
{
    private readonly RequestDelegate _next;

    public ApplicationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAppSettings appSettings)
    {
        if (context.Request.Path is { Value: not null, HasValue: true } && !context.Request.Path.Value.Contains("/api"))
        {
            await _next(context);
            return;
        }

        //BaseContext.UseContext(context);
        
        await CreateApplicationScope(context, appSettings);
        
        await _next(context);
    }

    private static async Task CreateApplicationScope(HttpContext context, IAppSettings appSettings)
    {
        if (!context.Request.Headers.TryGetValue("x-restaurant-id", out var restaurantIdFromHeader))
            throw new InvalidOperationException("Restaurant identification not informed!");

        var tenant = string.Empty;
        if (context.Request.Headers.TryGetValue("x-tenant-name", out var tenantHeader))
            tenant = tenantHeader;

        tenant ??= context.Request.Host.Host;
        var resGuid = Guid.Parse(restaurantIdFromHeader);

        var (tenantSettings, restaurantSettings) = await appSettings.GetSettingsAsync(tenant, resGuid);

        //TenantScope.CreateScope(tenantSettings);
        //RestaurantScope.CreateScope(restaurantSettings);
        
        context.Items["tenant_key"] = tenantSettings;
        context.Items["restaurant_key"] = restaurantSettings;
    }
}