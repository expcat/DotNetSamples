using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    public class SetLab
    {
        private static readonly string key = "lab:set";
        public static void SetMembers(IDatabase db)
        {
            db.SetAdd(key, 1);
            db.SetAdd(key, 2);
            db.SetAdd(key, 1);
            db.SetAdd(key, 3);
            System.Console.WriteLine("len:" + db.SetLength(key) + " " + string.Join("|", db.SetMembers(key)));
        }
    }
}
