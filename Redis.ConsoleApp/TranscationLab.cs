using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    public class TranscationLab
    {

        public static void LabTanscation(IDatabase db)
        {
            var tran = db.CreateTransaction();
            tran.StringSetAsync("lab:tran1", "tran1");
            tran.StringSetAsync("lab:tran2", "tran2");
            tran.ExecuteAsync();
            System.Console.WriteLine(db.StringGet("lab:tran1"));
            System.Console.WriteLine(db.StringGet("lab:tran2"));

            var batch = db.CreateBatch();
            batch.StringSetAsync("lab:batch1", "batch1");
            batch.StringSetAsync("lab:batch2", "batch2");
            batch.Execute();
            System.Console.WriteLine(db.StringGet("lab:batch1"));
            System.Console.WriteLine(db.StringGet("lab:batch2"));
        }
    }
}
