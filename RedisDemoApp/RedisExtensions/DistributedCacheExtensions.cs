using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedisDemoApp.RedisExtensions
{
    public static class DistributedCacheExtensions
    {
        // Extension Method
        public static async Task SetEntryAsync<T>(this IDistributedCache cache,
            string id,
            T data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? idleExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(120);
            options.SlidingExpiration = idleExpireTime;

            var jsonData = JsonSerializer.Serialize(data);

            await cache.SetStringAsync(id, jsonData, options);
        }


        // Extension Method
        public static async Task<T> GetEntryAsync<T>(this IDistributedCache cache, string id)
        {
            var jsonData = await cache.GetStringAsync(id);

            if (jsonData is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
