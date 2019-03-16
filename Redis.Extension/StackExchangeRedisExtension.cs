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

        #region Set

        public static bool Set<T>(this IDatabase db, string key, T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None) where T : class
        {
            return db.StringSet(key, MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance), expiry, when, flags);
        }

        public static Task<bool> SetAsync<T>(this IDatabase db, string key, T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None) where T : class
        {
            return db.StringSetAsync(key, MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance), expiry, when, flags);
        }

        public static bool SetLz4<T>(this IDatabase db, string key, T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None) where T : class
        {
            return db.StringSet(key, LZ4MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance), expiry, when, flags);
        }

        public static Task<bool> SetLz4Async<T>(this IDatabase db, string key, T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None) where T : class
        {
            return db.StringSetAsync(key, LZ4MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance), expiry, when, flags);
        }

        #endregion

        #region BlockPop

        public static RedisResult BlockLeftPop(this IDatabase db, RedisKey key, int timeout)
        {
            if (db.Multiplexer.TimeoutMilliseconds <= (timeout * 1000))
                throw new ArgumentException($"The {nameof(timeout)}({timeout}s) exceeded connection.TimeoutMilliseconds({db.Multiplexer.TimeoutMilliseconds}ms)");
            var result = db.Execute("blpop", key, timeout);
            if (result.IsNull)
                return result;
            else
                return ((RedisResult[]) result) [1];
        }

        public static async Task<RedisResult> BlockLeftPopAsync(this IDatabase db, RedisKey key, int timeout)
        {
            if (db.Multiplexer.TimeoutMilliseconds <= (timeout * 1000))
                throw new ArgumentException($"The {nameof(timeout)}({timeout}s) exceeded connection.TimeoutMilliseconds({db.Multiplexer.TimeoutMilliseconds}ms)");
            var result = await Task.Run(() => db.Execute("blpop", key, timeout));
            if (result.IsNull)
                return result;
            else
                return ((RedisResult[]) result) [1];
        }

        public static RedisResult BlockRightPop(this IDatabase db, RedisKey key, int timeout)
        {
            if (db.Multiplexer.TimeoutMilliseconds <= (timeout * 1000))
                throw new ArgumentException($"The {nameof(timeout)}({timeout}s) exceeded connection.TimeoutMilliseconds({db.Multiplexer.TimeoutMilliseconds}ms)");
            var result = db.Execute("brpop", key, timeout);
            if (result.IsNull)
                return result;
            else
                return ((RedisResult[]) result) [1];
        }

        public static async Task<RedisResult> BlockRightPopAsync(this IDatabase db, RedisKey key, int timeout)
        {
            if (db.Multiplexer.TimeoutMilliseconds <= (timeout * 1000))
                throw new ArgumentException($"The {nameof(timeout)}({timeout}s) exceeded connection.TimeoutMilliseconds({db.Multiplexer.TimeoutMilliseconds}ms)");
            var result = await Task.Run(() => db.Execute("brpop", key, timeout));
            if (result.IsNull)
                return result;
            else
                return ((RedisResult[]) result) [1];
        }

        #endregion
    }
}
