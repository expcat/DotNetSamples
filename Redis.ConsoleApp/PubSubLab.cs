using StackExchange.Redis;
using static StackExchange.Redis.RedisChannel;

namespace Redis.ConsoleApp
{
    public class PubSubLab
    {
        // private static readonly string channelKey = "lab:pubsub";
        public static void LabChannel(ConnectionMultiplexer con)
        {
            var sub = con.GetSubscriber();
            sub.Subscribe(new RedisChannel("mes?", PatternMode.Pattern), (channel, value) =>
            {
                System.Console.WriteLine($"channel:{channel}|value:{value}");
            });
            sub.Publish("mes1", "this is mes1.");
            sub.Publish("mes2", "this is mes2.");
            var db = con.GetDatabase(1);
            db.ScriptEvaluate("");
        }
    }
}
