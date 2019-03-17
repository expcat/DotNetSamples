using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.ConsoleApp
{
    public class TopicLab
    {

        private static readonly string exchange = "topicexchange";
        public static void TopicPublish(ConnectionFactory factory)
        {
            Console.WriteLine(" Press [enter] to begin.");
            Console.ReadLine();
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                for (int i = 0; i < 10; i++)
                {
                    string routingKey = i % 2 == 0 ? "a.com" : "b.cn";
                    var body = Encoding.UTF8.GetBytes("hello " + routingKey);
                    var prop = channel.CreateBasicProperties();
                    channel.BasicPublish(exchange, routingKey, null, body);
                    // Console.WriteLine("Sent: {0}", message);
                }
            }
            Console.WriteLine(" Press [Ctrl + C] to exit.");
        }

        public static void TopicReceive(ConnectionFactory factory, string index, string routingKey)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                string queue = "topicqueue_" + index;
                channel.ExchangeDeclare(exchange, ExchangeType.Topic, false, false, null);
                channel.QueueDeclare(queue, false, false, false, null);
                channel.QueueBind(queue, exchange, routingKey, null);
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    Console.WriteLine("[{0}] Received {1}", routingKey, message);
                };
                channel.BasicConsume(queue, true, consumer);
                Console.WriteLine("{0} receive started.", queue);
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
