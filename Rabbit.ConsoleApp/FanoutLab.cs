using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.ConsoleApp
{
    public class FanoutLab
    {
        private static readonly string exchange = "fanoutexchange";
        public static void FanoutPublish(ConnectionFactory factory)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Fanout, false, false, null);

                while (true)
                {
                    string message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message))
                        continue;
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange, string.Empty, null, body);
                    Console.WriteLine("Sent: {0}", message);
                }
            }
        }

        public static void FanoutReceive(ConnectionFactory factory, string index)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                string queue = "fanoutqueue_" + index;
                channel.QueueDeclare(queue, false, false, false, null);
                channel.QueueBind(queue, exchange, string.Empty, null);
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
