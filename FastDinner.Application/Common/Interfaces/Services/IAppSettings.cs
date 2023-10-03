namespace FastDinner.Application.Common.Interfaces.Services
{
    public interface IAppSettings
    {
        Task<TenantSettings> GetTenantSettingsAsync(string tenant);
    }
}
