using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace Rabbit.ConsoleApp
{
    public class RPCLab
    {
        private static readonly string exchange = "rpcexchange";
        private static readonly string routingKey = "rpclab";
        public static void RPCClient(ConnectionFactory factory)
        {
            Console.WriteLine(" Press [enter] to begin.");
            Console.ReadLine();
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                SimpleRpcClient client = new SimpleRpcClient(channel, exchange, ExchangeType.Direct, routingKey);
                for (int i = 0; i < 10; i++)
                {
                    var result = client.Call(Encoding.UTF8.GetBytes(i.ToString()));
                    Console.WriteLine($"Result: {Encoding.UTF8.GetString(result)}");
                }
            }
            Console.WriteLine(" Press [Ctrl + C] to exit.");
        }

        public static void RPCServer(ConnectionFactory factory)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                string queue = "rpcqueue";
                channel.ExchangeDeclare(exchange, ExchangeType.Direct, false, false, null);
                channel.QueueDeclare(queue, false, false, false, null);
                channel.QueueBind(queue, exchange, routingKey);
                Subscription sub = new Subscription(channel, queue);
                MyRpcServer server = new MyRpcServer(sub);
                Console.WriteLine("RPC server started.");
                server.MainLoop();
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }

    public class MyRpcServer : SimpleRpcServer
    {
        public MyRpcServer(Subscription subscription) : base(subscription) { }

        public override byte[] HandleSimpleCall(bool isRedelivered, IBasicProperties requestProperties, byte[] body, out IBasicProperties replyProperties)
        {
            replyProperties = null;
            var msg = $"已处理 {Encoding.UTF8.GetString(body)}";
            return Encoding.UTF8.GetBytes(msg);
        }
    }
}
