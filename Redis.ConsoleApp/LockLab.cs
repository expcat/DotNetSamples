using System;
using System.Threading;
using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    public class LockLab
    {
        private static readonly string key = "lab:lock";
        public static void LabLock(IDatabase db)
        {
            var val = Guid.NewGuid().ToString("N");
            System.Console.WriteLine(val);
            var isTake = db.LockTake(key, val, TimeSpan.FromSeconds(10));

            if (isTake)
            {
                System.Console.WriteLine("获取锁成功。");
                while (true)
                {
                    var live = db.KeyTimeToLive(key);
                    System.Console.WriteLine(live?.Seconds);
                    Thread.Sleep(1000);
                    if (live is null || live.Value.Seconds == 0)
                        break;
                }
                var isRelease = db.LockRelease(key, val);
                if (isRelease)
                    System.Console.WriteLine("释放锁成功。");
                else
                    System.Console.WriteLine("释放锁失败。");
            }
        }
    }
}
