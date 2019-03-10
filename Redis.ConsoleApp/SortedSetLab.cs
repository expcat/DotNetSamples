using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    public class SortedSetLab
    {
        private static readonly string key = "lab:sortetset";
        public static void LabSetGet(IDatabase db, int count)
        {
            for (int i = 0; i < count; i++)
            {
                db.SortedSetAdd(key, i, i, CommandFlags.FireAndForget);
            }

            var len = db.SortedSetLength(key);
            var rank = (long) (len * 0.1);

            var top10per = db.SortedSetRangeByRankWithScores(key, 0, rank, order : Order.Descending);
            System.Console.WriteLine(string.Join("|", top10per));
        }
    }
}
