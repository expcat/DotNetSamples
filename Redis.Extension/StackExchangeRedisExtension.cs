using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;
using StackExchange.Redis;

namespace Redis.Extension
{
    public static class StackExchangeRedisExtension
    {
        #region Get

        public static T Get<T>(this IDatabase db, string key) where T : class
        {
            var value = db.StringGet(key);
            if (!value.HasValue) return null;
            return MessagePackSerializer.Deserialize<T>(value, ContractlessStandardResolver.Instance);
        }

        public static async Task<T> GetAsync<T>(this IDatabase db, string key) where T : class
        {
            var value = await db.StringGetAsync(key);
            if (!value.HasValue) return null;
            return MessagePackSerializer.Deserialize<T>(value, ContractlessStandardResolver.Instance);
        }

        public static T GetLz4<T>(this IDatabase db, string key) where T : class
        {
            var value = db.StringGet(key);
            if (!value.HasValue) return null;
            return LZ4MessagePackSerializer.Deserialize<T>(value, ContractlessStandardResolver.Instance);
        }

        public static async Task<T> GetLz4Async<T>(this IDatabase db, string key) where T : class
        {
            var value = await db.StringGetAsync(key);
            if (!value.HasValue) return null;
            return LZ4MessagePackSerializer.Deserialize<T>(value, ContractlessStandardResolver.Instance);
        }

        #endregion
    }
}
