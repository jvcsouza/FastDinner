using System.Diagnostics.CodeAnalysis;
using FastDinner.Application.Common;
using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Common.Interfaces.Services;
#pragma warning disable VSTHRD002

namespace FastDinner.Infrastructure.Services
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AppSettings : IAppSettings
    {
        private const string SUFIX = "_settings";
        private readonly ITableStore _tableStore;
        private readonly ICacheProvider _cache;
        private readonly Lazy<IRestaurantRepository> _restaurantRepository;

        public AppSettings(ITableStore tableStore, ICacheProvider cache, Lazy<IRestaurantRepository> restaurantRepository)
        {
            _tableStore = tableStore;
            _cache = cache;
            _restaurantRepository = restaurantRepository;

            _tableStore.CreateIfNotExistsAsync()
                .GetAwaiter().GetResult();
        }

        public async Task<TenantSettings> GetTenantSettingsAsync(string tenant)
        {
            return await _cache.GetOrAddAsync($"{tenant}{SUFIX}", async () =>
            {
                var lst = await _tableStore.GetAllPartitionsAsync<AppTenantSettings>($"{tenant}{SUFIX}");

                var setting = lst.FirstOrDefault();

                if (setting?.Settings is null)
                    throw new InvalidOperationException("Tenant not found!");
                
                return setting?.Settings;

            });
        }

        public async Task<(TenantSettings Tenant, RestaurantSettings Restaurant)> GetSettingsAsync(string tenant, Guid restaurantId)
        {
            var tenantSettings = await GetTenantSettingsAsync(tenant);

            var restaurantSettings = await _cache.GetOrAddAsync($"{tenantSettings.Dns}{restaurantId}", async () =>
            {
                var restaurant = await _restaurantRepository.Value.GetByIdAsync(restaurantId);

                switch (restaurant)
                {
                    case null:
                        throw new InvalidOperationException("Restaurant not found!");

                    default:
                        {
                            var resScope = new RestaurantSettings { Name = restaurant.Name, ResturantId = restaurant.Id };
                            return resScope;
                        }
                }
            });

            return (tenantSettings, restaurantSettings); ;
        }
    }
}
