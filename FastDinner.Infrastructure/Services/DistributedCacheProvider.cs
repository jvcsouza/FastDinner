using FastDinner.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace FastDinner.Infrastructure.Services
{
    public class DistributedCacheProvider : ICacheProvider
    {
        private readonly IDistributedCache _distributedCache;

        public DistributedCacheProvider(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public DistributedCacheEntryOptions GetCacheEntryOptions(TimeSpan? expiration = null)
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(15)
            };
        }

        public T GetOrAdd<T>(string key, Func<T> createItem, TimeSpan? expiration = null)
        {
            var cachedItem = _distributedCache.GetString(key);

            if (!string.IsNullOrEmpty(cachedItem))
                return JsonConvert.DeserializeObject<T>(cachedItem);

            // If the item is not in cache, create it
            var cacheEntry = createItem();

            if (cacheEntry == null) return default(T);

            _distributedCache.SetString(key, JsonConvert.SerializeObject(cacheEntry), GetCacheEntryOptions(expiration));

            return cacheEntry;
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> createItem, TimeSpan? expiration = null)
        {
            var cachedItem = await _distributedCache.GetStringAsync(key);

            if (!string.IsNullOrEmpty(cachedItem))
                return JsonConvert.DeserializeObject<T>(cachedItem);

            var cacheEntry = await createItem();

            if (cacheEntry == null) return default(T);

            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(cacheEntry), GetCacheEntryOptions(expiration));

            return cacheEntry;
        }

        public void RemoveBase(string baseKey)
        {
            _distributedCache.Remove(baseKey);
        }
    }
}
