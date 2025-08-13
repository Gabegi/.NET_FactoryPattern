using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Services;

public class MemoryCacheService : ICacheService
{
    private readonly Dictionary<string, (object Value, DateTime Expiration)> _cache = new();

    public Task<T?> GetAsync<T>(string key)
    {
        if (_cache.TryGetValue(key, out var cachedItem))
        {
            if (cachedItem.Expiration > DateTime.UtcNow)
            {
                return Task.FromResult<T?>((T)cachedItem.Value);
            }
            else
            {
                _cache.Remove(key);
            }
        }
        return Task.FromResult<T?>(default);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var expirationTime = DateTime.UtcNow.Add(expiration ?? TimeSpan.FromMinutes(30));
        _cache[key] = (value!, expirationTime);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string key)
    {
        return Task.FromResult(_cache.ContainsKey(key) && _cache[key].Expiration > DateTime.UtcNow);
    }
}
