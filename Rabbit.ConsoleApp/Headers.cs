using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.ConsoleApp
{
    public class Headers
    {
        private static readonly string exchange = "headersexchange";
        public static void HeadersPublish(ConnectionFactory factory)
        {
            Console.WriteLine(" Press [enter] to begin.");
            Console.ReadLine();
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Headers, false, false, null);
                var headers_0 = new Dictionary<string, object>() { { "name", "rabbit" }, { "bit", "true" } };
                var headers_1 = new Dictionary<string, object>() { { "name", "rabbit" }, { "bit", "false" } };
                for (int i = 0; i < 100; i++)
                {
                    var body = Encoding.UTF8.GetBytes("hello " + i);
                    var prop = channel.CreateBasicProperties();
                    prop.Headers = i % 2 == 0 ? headers_0 : headers_1;
                    channel.BasicPublish(exchange, string.Empty, prop, body);
                    // Console.WriteLine("Sent: {0}", message);
                }
            }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public static void HeadersReceive(ConnectionFactory factory, string index, bool isAny)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                string queue = "headersqueue_" + index;
                channel.QueueDeclare(queue, false, false, false, null);
                string match = isAny ? "any" : "all";
                channel.QueueBind(queue, exchange, string.Empty, new Dictionary<string, object>()
                { { "x-match", match }, { "name", "rabbit" }, { "bit", "true" }
                });
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    Console.WriteLine("[{0}] Received {1}", index, message);
                };
                channel.BasicConsume(queue, true, consumer);
                Console.WriteLine("{0} receive started.", queue);
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
