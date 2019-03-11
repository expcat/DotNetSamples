using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    public class HyperLogLab
    {
        private static readonly string key = "lab:hyperlog";
        public static void LabCount(IDatabase db, int count)
        {
            var batch = db.CreateBatch();
            batch.HyperLogLogAddAsync(key, 1, CommandFlags.FireAndForget);
            batch.HyperLogLogAddAsync(key, 2, CommandFlags.FireAndForget);
            batch.HyperLogLogAddAsync(key, 1, CommandFlags.FireAndForget);
            batch.HyperLogLogAddAsync(key, 3, CommandFlags.FireAndForget);
            batch.Execute();
            System.Console.WriteLine($"total:4|result:{db.HyperLogLogLength(key)}");
            for (int i = 0; i < count; i++)
            {
                batch.HyperLogLogAddAsync(key, i, CommandFlags.FireAndForget);
            }
            batch.Execute();
            System.Console.WriteLine($"total:{count}|result:{db.HyperLogLogLength(key)}");
        }
    }
}
