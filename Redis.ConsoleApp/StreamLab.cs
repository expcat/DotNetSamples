using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    public class StreamLab
    {
        public static void LabStream(IDatabase db)
        {
            string key = "lab:stream";
            var cts = new CancellationTokenSource();
            Task.Run(() =>
            {
                RedisValue point = 0;
                while (true)
                {
                    if (cts.IsCancellationRequested)
                    {
                        break;
                    }
                    var result = db.StreamRead(key, point, 1);
                    if (result.Length == 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    point = result[0].Id;
                    System.Console.WriteLine(result[0].Values[0]);
                    db.StreamDelete(key, new RedisValue[] { point });
                }
            }, cts.Token);
            for (int i = 0; i < 10; i++)
            {
                string val = "stream" + i;
                db.StreamAdd(key, val, val);
                Thread.Sleep(1000);
            }
            cts.Cancel();
            cts.Dispose();
        }

        public static void LabCustomer(IDatabase db)
        {
            string key = "lab:customer";
            string group = "customer";
            var batch = db.CreateBatch();
            for (int i = 0; i < 100; i++)
            {
                batch.StreamAddAsync(key, "stream", i);
            }
            batch.Execute();
            db.StreamCreateConsumerGroup(key, group, StreamPosition.Beginning);
            for (int i = 0; i < 2; i++)
            {
                Task.Factory.StartNew((index) =>
                {
                    string customer = "expcat" + index;
                    System.Console.WriteLine(customer + "开始...");
                    while (true)
                    {
                        var result = db.StreamReadGroup(key, group, customer, StreamPosition.NewMessages, 1);
                        if (result.Length == 0)
                        {
                            Thread.Sleep(100);
                            continue;
                        }
                        var point = result[0].Id;
                        System.Console.WriteLine(customer + result[0].Values[0]);
                        db.StreamAcknowledge(key, group, point);
                        Thread.Sleep(100);
                    }
                }, i);
            }
        }
    }
}
