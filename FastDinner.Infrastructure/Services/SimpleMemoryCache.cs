using System.Runtime.Caching;
using FastDinner.Application.Common.Interfaces.Services;
using Microsoft.VisualStudio.Threading;

namespace FastDinner.Infrastructure.Services;

// Provide JoinableTaskFactory where allowed
#pragma warning disable VSTHRD012 
public class SimpleMemoryCache : ICacheProvider
{
    private readonly AsyncReaderWriterLock _lock = new();
    public static readonly object LockSimpleCache = new();
    protected MemoryCache Cache { get; }
    private CacheItemPolicy _policy;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SimpleMemoryCache(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        Cache = MemoryCache.Default;
    }

    private CacheItemPolicy GetPolicy(TimeSpan? expiration)
    {
        if (expiration.HasValue)
        {
            return new CacheItemPolicy { AbsoluteExpiration = _dateTimeProvider.UtcNow.Add(expiration.Value) };
        }

        return _policy ??= new CacheItemPolicy { AbsoluteExpiration = _dateTimeProvider.UtcNow.AddMinutes(15) };
    }

    public T GetOrAdd<T>(string key, Func<T> createItem, TimeSpan? expiration)
    {
        if (Cache.Contains(key))
            return (T)Cache.Get(key);

        lock (LockSimpleCache)
        {
            if (Cache.Contains(key))
                return (T)Cache.Get(key);

            var cacheEntry = createItem();

            if (cacheEntry != null)
                Cache.Add(key, cacheEntry, GetPolicy(expiration));

            return cacheEntry;
        }
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> createItem, TimeSpan? expiration)
    {
        if (Cache.Contains(key))
            return (T)Cache.Get(key);

        await using (await _lock.WriteLockAsync())
        {
            if (Cache.Contains(key))
                return (T)Cache.Get(key);

            var cacheEntry = await createItem();

            if (cacheEntry != null)
                Cache.Add(key, cacheEntry, GetPolicy(expiration));

            return cacheEntry;
        }
    }

    public void RemoveBase(string baseKey)
    {
        var cacheKeys = Cache.Select(kvp => kvp.Key).Where(w => w.Contains(baseKey)).ToList();

        if (cacheKeys.Count <= 0) return;

        lock (LockSimpleCache)
        {
            foreach (var key in cacheKeys)
            {
                if (Cache.Contains(key))
                {
                    Cache.Remove(key);
                }
            }
        }

    }
}