using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    public class HashLab
    {
        private static readonly string key = "lab:hash";

        public static void LabSetGet(IDatabase db)
        {
            db.HashSet(key, 1, "val1");
            db.HashSet(key, 2, "val2");
            System.Console.WriteLine(db.HashGet(key, 1));
            System.Console.WriteLine(db.HashLength(key));
            System.Console.WriteLine(string.Join("|", db.HashKeys(key)));
            System.Console.WriteLine(string.Join("|", db.HashGetAll(key)));
        }
    }
}
