namespace FastDinner.Application.Common.Interfaces.Services;

public interface ICacheProvider
{
    T GetOrAdd<T>(string key, Func<T> createItem, TimeSpan? expiration = null);
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> createItem, TimeSpan? expiration = null);
    void RemoveBase(string baseKey);
}

public static class CacheKeys
{
    
}