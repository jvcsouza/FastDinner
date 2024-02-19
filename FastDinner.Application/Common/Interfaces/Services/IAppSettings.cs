namespace FastDinner.Application.Common.Interfaces.Services
{
    public interface IAppSettings
    {
        Task<TenantSettings> GetTenantSettingsAsync(string tenant);
        Task<(TenantSettings Tenant, RestaurantSettings Restaurant)> GetSettingsAsync(string tenant, Guid restaurantId);
    }
}
