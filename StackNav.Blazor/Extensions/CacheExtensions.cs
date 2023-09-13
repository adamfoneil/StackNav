using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace StackNav.Blazor.Extensions;

public static class DistributedCacheExtensions
{
    public static async Task<T> GetItemAsync<T>(this IDistributedCache cache, string key)
    {
        var bytes = await cache.GetAsync(key);
        if (bytes == null) return default!;
        var json = Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize<T>(json)!;
    }

    public static T GetItem<T>(this IDistributedCache cache, string key)
    {
        var bytes = cache.Get(key);
        if (bytes == null) return default!;
        var json = Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize<T>(json)!;
    }

    public static async Task SetItemAsync<T>(this IDistributedCache cache, string key, T data, DistributedCacheEntryOptions? options = null)
    {
        var json = JsonSerializer.Serialize(data);
        var bytes = Encoding.UTF8.GetBytes(json);
        await cache.SetAsync(key, bytes, options ?? new());
    }

    public static void SetItem<T>(this IDistributedCache cache, string key, T data, DistributedCacheEntryOptions? options = null)
    {
        var json = JsonSerializer.Serialize(data);
        var bytes = Encoding.UTF8.GetBytes(json);
        cache.Set(key, bytes, options ?? new());
    }
}
