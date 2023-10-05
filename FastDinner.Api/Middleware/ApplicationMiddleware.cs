using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FastDinner.Application.Common.Interfaces.Services;
using FastDinner.Application.Common;
using FastDinner.Application.Common.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FastDinner.Api.Middleware;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class ApplicationMiddleware
{
    private readonly RequestDelegate _next;

    public ApplicationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAppSettings appSettings, IServiceProvider provider)
    {
        BaseContext.UseContext(context);
        await CreateApplicationScope(context, appSettings, provider);
        await _next(context);
    }

    private static async Task CreateApplicationScope(HttpContext context, IAppSettings appSettings,
        IServiceProvider serviceProvider)
    {
        if (!context.Request.Headers.TryGetValue("x-restaurant-id", out var restaurantIdHeader))
            throw new InvalidOperationException("Restaurant identification not informed!");

        var tenant = string.Empty;
        if (context.Request.Headers.TryGetValue("x-tenant-name", out var tenantHeader))
            tenant = tenantHeader;

        tenant ??= context.Request.Host.Host;

        var settings = await appSettings.GetTenantSettingsAsync(tenant);

        if (settings is null)
            throw new InvalidOperationException("Tenant not found!");

        TenantScope.CreateScope(settings);

        await using var scope = serviceProvider.CreateAsyncScope();
        var cacheProvider = scope.ServiceProvider.GetService<ICacheProvider>();
        var resGuid = Guid.Parse(restaurantIdHeader);

        var restaurantScope = await cacheProvider.GetOrAddAsync($"{settings.Dns}{resGuid}", async () =>
        {
            // ReSharper disable once AccessToDisposedClosure
            var resRepository = scope.ServiceProvider.GetService<IRestaurantRepository>();
            var restaurant = await resRepository.GetByIdAsync(resGuid);

            if (restaurant is null)
                throw new InvalidOperationException("Restaurant not found!");

            var resScope = new RestaurantSettings { Name = restaurant.Name, ResturantId = restaurant.Id };

            return resScope;

        }).ConfigureAwait(true);

        RestaurantScope.CreateScope(restaurantScope);
    }
}