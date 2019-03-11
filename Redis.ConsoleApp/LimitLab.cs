using System.Threading;
using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    public class LimitLab
    {
        public static void LabLimit(IDatabase db)
        {
            string script = @"
                local limit = redis.call('incr',KEYS[1]);
                if limit == 1 then
                    redis.call('expire',KEYS[1],1);
                else
                    if limit > tonumber(ARGV[1]) then
                        return 0;
                    end
                end
                return 1;";
            for (int i = 0; i < 100; i++)
            {
                var result = db.ScriptEvaluate(script, new RedisKey[] { "lab:limit" }, new RedisValue[] { 10 });
                System.Console.WriteLine(result);
                Thread.Sleep(10);
            }

        }
    }
}
