using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Redis.Extension;
using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    public class ListLab
    {
        private static readonly string key = "lab:list";
        public static void LabPushPop(IDatabase db)
        {
            db.ListLeftPush(key, 1);
            var val = db.ListRightPop(key);
            System.Console.WriteLine(val);
        }

        public static void LabPushPopWait(IDatabase db)
        {
            db.ListLeftPush(key, 1);
            db.ListLeftPush(key, 2);
            db.ListLeftPush(key, 3);
            db.ListLeftPush(key, 4);
            while (true)
            {
                var val = db.ListRightPop(key);
                if (!val.HasValue) break;
                System.Console.WriteLine(val);
                Thread.Sleep(1000);
            }
            System.Console.WriteLine("List is empty.");
        }

        public static void LabPushPopTest(IDatabase db)
        {
            for (int i = 0; i < 1000; i++)
            {
                db.ListLeftPush(key, i);
            }
            int t = 5;
            Task[] tasks = new Task[t];
            int sum = 0;
            for (int i = 0; i < t; i++)
            {
                tasks[i] = Task.Factory.StartNew((index) =>
                {
                    System.Console.WriteLine($"Start:{index}");
                    ConnectionMultiplexer con = ConnectionMultiplexer.Connect($"{ConfigHelper.host},password={ConfigHelper.password}");
                    var db_temp = con.GetDatabase(1);
                    int total = 0;
                    while (true)
                    {
                        var val = db_temp.ListRightPop(key);
                        if (!val.HasValue) break;
                        total += 1;
                    }
                    System.Console.WriteLine($"Finished:{index}|Total:{total}");
                    sum = Interlocked.Add(ref sum, total);
                }, i);
            }
            Task.WaitAll(tasks);
            System.Console.WriteLine("End. Sum:" + sum);
        }

        public static void LabInsteadBPushPopTest(ConnectionMultiplexer con, IDatabase db)
        {
            string channel = "list_notify";
            var sub = con.GetSubscriber();
            sub.Subscribe(channel, (c, v) =>
            {
                var work = db.ListRightPop(key);
                if (work.HasValue) System.Console.WriteLine(work);
            });
            var batch = db.CreateBatch();
            for (int i = 0; i < 10; i++)
            {
                batch.ListLeftPushAsync(key, "work" + i);
                batch.PublishAsync(channel, "");
                // db.ListLeftPush(key, "work" + i);
                // sub.Publish(channel, "");
                batch.Execute();
                Thread.Sleep(1000);
            }
        }

        public static void labBPushPopTest(IDatabase db)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    db.ListRightPush(key, i);
                    // System.Console.WriteLine($"rpush {i}");
                    Thread.Sleep(8000);
                }
            });
            ConnectionMultiplexer con = ConnectionMultiplexer.Connect($"{ConfigHelper.host},password={ConfigHelper.password},allowAdmin=true,syncTimeout=20000");
            System.Console.WriteLine(con.TimeoutMilliseconds);
            var db_new = con.GetDatabase(1);
            while (true)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var result = db_new.Execute("blpop", key, 10);
                sw.Stop();
                if (result.IsNull)
                {
                    System.Console.WriteLine($"blpop null | {sw.ElapsedMilliseconds}ms");
                }
                else
                {
                    if (result.Type == ResultType.MultiBulk)
                    {
                        RedisResult[] results = (RedisResult[]) result;
                        System.Console.WriteLine($"blpop {results[0]} | {results[1]} | {sw.ElapsedMilliseconds}ms");
                    }
                }
            }
        }
    }
}
