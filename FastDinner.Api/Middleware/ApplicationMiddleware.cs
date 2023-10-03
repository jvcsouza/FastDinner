using System;
using System.Threading.Tasks;
using FastDinner.Api.Services;
using Microsoft.AspNetCore.Http;
using FastDinner.Application.Common.Interfaces.Services;
using FastDinner.Application.Common;
using FastDinner.Application.Common.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FastDinner.Api.Middleware;

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

    private async Task CreateApplicationScope(HttpContext context, IAppSettings appSettings,
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

        //TenantScope.CreateScope(settings);

        await using var scope = serviceProvider.CreateAsyncScope();
        var cacheProvider = scope.ServiceProvider.GetService<ICacheProvider>();
        var appScope = scope.ServiceProvider.GetService<AppScope>();
        var resGuid = Guid.Parse(restaurantIdHeader);

        appScope.UseTenant(settings);

        var restaurantScope = await cacheProvider.GetOrAddAsync($"{settings.Dns}{resGuid}", async () =>
        {
            var resRepository = scope.ServiceProvider.GetService<IRestaurantRepository>();
            var restaurant = await resRepository.GetByIdAsync(resGuid);

            if (restaurant is null)
                throw new InvalidOperationException("Restaurant not found!");

            var resScope = new RestaurantSettings() { Name = restaurant.Name, ResturantId = restaurant.Id };

            return resScope;

        }).ConfigureAwait(true);

        //RestaurantScope.CreateScope(restaurantScope);

        appScope.UseRestaurant(restaurantScope);
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        throw new NotImplementedException();
    }
}