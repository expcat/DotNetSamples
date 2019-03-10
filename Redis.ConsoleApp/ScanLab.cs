using Redis.Extension;
using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    public class ScanLab
    {
        public static void LabScan(IDatabase db, ConnectionMultiplexer con)
        {
            var server = con.GetServer(ConfigHelper.host);
            for (int i = 0; i < 100; i++)
            {
                db.StringSet("lab:string" + i, "str" + i, flags : CommandFlags.FireAndForget);
                db.StringSet("lab:int" + i, i, flags : CommandFlags.FireAndForget);
            }
            var list = server.Keys(1, "lab:*", 10);
            foreach (var item in list)
            {
                System.Console.WriteLine(item);
            }
        }
    }
}
