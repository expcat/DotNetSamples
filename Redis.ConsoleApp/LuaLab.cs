using System;
using System.Text;
using Redis.Extension;
using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    public class LuaLab
    {
        public static void LabLua(IDatabase db, ConnectionMultiplexer con)
        {
            var result1 = db.ScriptEvaluate("return KEYS[1] + KEYS[2];", new RedisKey[] { "1", "3" });
            System.Console.WriteLine(result1);

            string key = "lab:luaHash";
            db.HashSet(key, "age1", 20);
            db.HashSet(key, "age2", 25);
            db.HashSet(key, "age3", 28);
            db.HashSet(key, "age4", 30);
            db.HashSet(key, "age5", 35);
            string script = @"
                local count=0;
                local hkeys=redis.call('hkeys',KEYS[1]);
                for k,v in pairs(hkeys) do
                    local hval=redis.call('hget',KEYS[1],v);
                    if(tonumber(hval) > tonumber(ARGV[1])) then
                        redis.call('hdel',KEYS[1],v);
                        count=1+count;
                    end
                end
                return count;";
            var lua = LuaScript.Prepare(script).Load(con.GetServer(ConfigHelper.host));
            System.Console.WriteLine(BitConverter.ToString(lua.Hash));
            System.Console.WriteLine(Convert.ToBase64String(lua.Hash));
            var result2 = db.ScriptEvaluate(lua.Hash, new RedisKey[] { key }, new RedisValue[] { 25 });
            System.Console.WriteLine(result2);

            System.Console.WriteLine(string.Join("|", db.HashGetAll(key)));
        }
    }
}
