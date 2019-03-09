using Microsoft.Extensions.Caching.Distributed;
using System;
using MessagePack;
using System.Threading.Tasks;
using MessagePack.Resolvers;
using System.Threading;

namespace Redis.Extension
{
    public static class CacheExtension
    {
        #region Get

        public static T Get<T>(this IDistributedCache cache, string key) where T : class
        {
            var value = cache.Get(key);
            if (value == null) return null;
            return MessagePackSerializer.Deserialize<T>(value, ContractlessStandardResolver.Instance);
        }

        public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key, CancellationToken token = default (CancellationToken)) where T : class
        {
            var value = await cache.GetAsync(key, token);
            if (value == null) return null;
            return MessagePackSerializer.Deserialize<T>(value, ContractlessStandardResolver.Instance);
        }

        public static T GetLz4<T>(this IDistributedCache cache, string key) where T : class
        {
            var value = cache.Get(key);
            if (value == null) return null;
            return LZ4MessagePackSerializer.Deserialize<T>(cache.Get(key), ContractlessStandardResolver.Instance);
        }

        public static async Task<T> GetLz4Async<T>(this IDistributedCache cache, string key, CancellationToken token = default (CancellationToken)) where T : class
        {
            var value = await cache.GetAsync(key, token);
            if (value == null) return null;
            return LZ4MessagePackSerializer.Deserialize<T>(await cache.GetAsync(key, token), ContractlessStandardResolver.Instance);
        }

        #endregion

        #region Set

        public static void Set<T>(this IDistributedCache cache, string key, T value) where T : class
        {
            cache.Set(key, MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance), new DistributedCacheEntryOptions());
        }

        public static void Set<T>(this IDistributedCache cache, string key, T value, TimeSpan slidingExpiration) where T : class
        {
            cache.Set(key, MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance), new DistributedCacheEntryOptions().SetSlidingExpiration(slidingExpiration));
        }

        public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken token = default (CancellationToken)) where T : class
        {
            return cache.SetAsync(key, MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance), new DistributedCacheEntryOptions(), token);
        }

        public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, TimeSpan slidingExpiration, CancellationToken token = default (CancellationToken)) where T : class
        {
            return cache.SetAsync(key, MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance), new DistributedCacheEntryOptions().SetSlidingExpiration(slidingExpiration), token);
        }

        public static void SetLz4<T>(this IDistributedCache cache, string key, T value) where T : class
        {
            cache.Set(key, LZ4MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance), new DistributedCacheEntryOptions());
        }

        public static void SetLz4<T>(this IDistributedCache cache, string key, T value, TimeSpan slidingExpiration) where T : class
        {
            cache.Set(key, LZ4MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance), new DistributedCacheEntryOptions().SetSlidingExpiration(slidingExpiration));
        }

        public static Task SetLz4Async<T>(this IDistributedCache cache, string key, T value, CancellationToken token = default (CancellationToken)) where T : class
        {
            return cache.SetAsync(key, LZ4MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance), new DistributedCacheEntryOptions(), token);
        }

        public static Task SetLz4Async<T>(this IDistributedCache cache, string key, T value, TimeSpan slidingExpiration, CancellationToken token = default (CancellationToken)) where T : class
        {
            return cache.SetAsync(key, LZ4MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance), new DistributedCacheEntryOptions().SetSlidingExpiration(slidingExpiration), token);
        }

        #endregion
    }
}
