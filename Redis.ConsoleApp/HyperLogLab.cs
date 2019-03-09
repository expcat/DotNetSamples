using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    public class HyperLogLab
    {
        private static readonly string key = "lab:hyperlog";
        public static void CountLab(IDatabase db, int count)
        {
            db.HyperLogLogAdd(key, 1, CommandFlags.FireAndForget);
            db.HyperLogLogAdd(key, 2, CommandFlags.FireAndForget);
            db.HyperLogLogAdd(key, 1, CommandFlags.FireAndForget);
            db.HyperLogLogAdd(key, 3, CommandFlags.FireAndForget);
            System.Console.WriteLine($"total:4|result:{db.HyperLogLogLength(key)}");
            for (int i = 0; i < count; i++)
            {
                db.HyperLogLogAdd(key, i, CommandFlags.FireAndForget);
            }
            System.Console.WriteLine($"total:{count}|result:{db.HyperLogLogLength(key)}");
        }
    }
}
