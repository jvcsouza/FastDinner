using FastDinner.Application.Common;
using FastDinner.Application.Common.Interfaces.Services;
#pragma warning disable VSTHRD002

namespace FastDinner.Infrastructure.Services
{
    public class AppSettings : IAppSettings
    {
        private const string SUFIX = "_settings";
        private readonly ITableStore _tableStore;
        private readonly ICacheProvider _cache;

        public AppSettings(ITableStore tableStore, ICacheProvider cache)
        {
            _tableStore = tableStore;
            _cache = cache;

            _tableStore.CreateIfNotExistsAsync()
                .GetAwaiter().GetResult();
        }

        public async Task<TenantSettings> GetTenantSettingsAsync(string tenant)
        {
            return await _cache.GetOrAddAsync($"{tenant}{SUFIX}", async () =>
            {
                var lst = await _tableStore.GetAllPartitionsAsync<AppTenantSettings>($"{tenant}{SUFIX}");

                var setting = lst.FirstOrDefault();

                return setting?.Settings;

            });
        }
    }
}
