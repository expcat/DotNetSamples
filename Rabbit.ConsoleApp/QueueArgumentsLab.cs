using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.ConsoleApp
{
    public class QueueArgumentsLab
    {
        public static void QueueTTLLab(ConnectionFactory factory)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                string queue = "queuettlqueue";
                channel.QueueDeclare(queue, false, false, false, new Dictionary<string, object> { { "x-message-ttl", 5000 } });
                channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes("hello"));
                Console.WriteLine("Send finished");
            }
        }
        public static void MessageTTLLab(ConnectionFactory factory)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                string queue = "msgttlqueue";
                channel.QueueDeclare(queue, false, false, false, null);
                var prop = channel.CreateBasicProperties();
                prop.Expiration = "5000";
                channel.BasicPublish(string.Empty, queue, prop, Encoding.UTF8.GetBytes("hello"));
                Console.WriteLine("Send finished");
            }
        }
        public static void ExpireLab(ConnectionFactory factory)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                string queue = "expirequeue";
                channel.QueueDeclare(queue, false, false, false, new Dictionary<string, object> { { "x-expires", 5000 } });
                Console.WriteLine("Send finished");
            }
        }
        public static void MaxLengthLab(ConnectionFactory factory)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                string queue = "maxlenqueue";
                channel.QueueDeclare(queue, false, false, false, new Dictionary<string, object> { { "x-max-length", 10 } });
                for (int i = 0; i < 15; i++)
                {
                    channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes("hello" + i));
                }
                Console.WriteLine("Send finished");
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    Console.WriteLine("Received {0}", message);
                };
                channel.BasicConsume(queue, true, consumer);
            }
        }
        public static void DeadLetterLab(ConnectionFactory factory)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                string exchange = "deadexchange";
                string routingKey = "msgttl";
                channel.ExchangeDeclare(exchange, ExchangeType.Direct, false, false, null);
                string letterqueue = "letterqueue";
                channel.QueueDeclare(letterqueue, false, false, false, null);
                channel.QueueBind(letterqueue, exchange, routingKey, null);

                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    Console.WriteLine("Received {0}", message);
                };
                channel.BasicConsume(letterqueue, true, consumer);
                Console.WriteLine("Consumer watched.");

                string deadqueue = "deadqueue";
                channel.QueueDeclare(deadqueue, false, false, false, new Dictionary<string, object> { { "x-message-ttl", 5000 }, { "x-dead-letter-exchange", exchange }, { "x-dead-letter-routing-key", routingKey } });
                channel.BasicPublish(string.Empty, deadqueue, null, Encoding.UTF8.GetBytes("hello"));
                Console.WriteLine("Send finished");
                Console.ReadKey();
            }
        }
        public static void MaxPriorityLab(ConnectionFactory factory)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                string queue = "maxprioqueue";
                channel.QueueDeclare(queue, false, false, false, new Dictionary<string, object> { { "x-max-priority", 10 } });
                var batch = channel.CreateBasicPublishBatch();
                for (int i = 0; i < 10; i++)
                {
                    var prop = channel.CreateBasicProperties();
                    prop.Priority = (byte) i;
                    batch.Add(string.Empty, queue, false, prop, Encoding.UTF8.GetBytes("hello" + i));
                }
                batch.Publish();
                Console.WriteLine("Send finished");
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    Console.WriteLine("Received {0}", message);
                };
                channel.BasicConsume(queue, true, consumer);
                Console.ReadKey();
            }
        }
    }
}
