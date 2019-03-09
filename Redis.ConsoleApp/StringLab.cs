using System;
using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    public class StringLab
    {
        public static void LabIncrement(IDatabase db)
        {
            int maxCount = 3000;
            while (true)
            {
                var num = db.StringIncrement("lab:max");
                //Console.Write(num + "|");
                if (num > maxCount)
                {
                    Console.WriteLine($"当前请求大于{maxCount}");
                    break;
                }
            }
        }

        public static void LabIsExists(IDatabase db)
        {
            while (true)
            {
                var result = db.StringSet("lab:exists", "aa", TimeSpan.FromSeconds(10), When.NotExists);
                Console.WriteLine(result);
                if (!result) break;
            }
        }

        public static void LabBit(IDatabase db)
        {
            string key = "lab:bit";
            db.StringSetBit(key, 1, true);
            db.StringSetBit(key, 2, true);
            db.StringSetBit(key, 3, true);

            Console.WriteLine($"2:{db.StringGetBit(key, 2)}|4:{db.StringGetBit(key, 4)}");
        }
    }
}
