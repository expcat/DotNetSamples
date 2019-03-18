using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.ConsoleApp
{
    public class MessageControl
    {
        public static void AckLab(ConnectionFactory factory)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                string queue = "ackqueue";
                channel.QueueDeclare(queue, false, false, false, null);
                // 1 confirm机制
                channel.ConfirmSelect();
                for (int i = 0; i < 5; i++)
                {
                    channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes("hello" + i));
                }
                channel.WaitForConfirms(TimeSpan.FromSeconds(10));

                // 2 tx机制
                // try
                // {
                //     channel.TxSelect();
                //     for (int i = 0; i < 5; i++)
                //     {
                //         channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes("hello" + i));
                //     }
                //     channel.TxCommit();
                // }
                // catch (Exception)
                // {
                //     channel.TxRollback();
                // }
                // Console.WriteLine("Send finished");

                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    Console.WriteLine("Received {0}", message);
                    // 1 消息确认
                    channel.BasicAck(ea.DeliveryTag, false);
                    // 2.1 拒收，重新放回队列
                    // channel.BasicNack(ea.DeliveryTag, false, true);
                    // 2.2 拒收，丢弃
                    // channel.BasicNack(ea.DeliveryTag, false, false);
                    // 3 重发
                    // channel.BasicRecover(true);
                };
                channel.BasicConsume(queue, false, consumer);
                Console.ReadKey();
            }
        }

        public static void PropLab(ConnectionFactory factory)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                // 1 消息持久化 （Default Queue + Disk）
                string queue1 = "prop1queue";
                channel.QueueDeclare(queue1, false, false, false, null);
                var prop = channel.CreateBasicProperties();
                prop.Persistent = true;
                for (int i = 0; i < 5; i++)
                {
                    channel.BasicPublish(string.Empty, queue1, prop, Encoding.UTF8.GetBytes("default" + i));
                }

                // 2 Lazy Queue + In Memory (不耗费内存，但消息并不持久化，性能稍差) 【Default Queue + IN Memory (耗费内存，性能最好)】
                string queue2 = "prop2queue";
                channel.QueueDeclare(queue2, false, false, false, new Dictionary<string, object> { { "x-queue-mode", "lazy" } });
                for (int i = 0; i < 5; i++)
                {
                    channel.BasicPublish(string.Empty, queue1, null, Encoding.UTF8.GetBytes("lazy" + i));
                }

                // 3 Lazy Queue + Disk (不耗费内存，消息会持久化) 【Default Queue + Disk (耗费内存，消息会持久化)】
                channel.QueueDeclare(queue2, false, false, false, new Dictionary<string, object> { { "x-queue-mode", "lazy" } });
                for (int i = 0; i < 5; i++)
                {
                    channel.BasicPublish(string.Empty, queue1, prop, Encoding.UTF8.GetBytes("lazy+disk" + i));
                }
            }
        }

        public static void ConsumerLab(ConnectionFactory factory)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                string queue = "consumerqueue";
                channel.QueueDeclare(queue, false, false, false, null);
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

                // QOS（限制每次取数据大小，长度，是否为通用设置）
                channel.BasicQos(0, 1, false);

                consumer.Received += (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    Console.WriteLine("Received {0}", message);
                    // Ack（不管是否Ack，消息都会根据QOS限制放到Consumer中）
                    channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume(queue, false, consumer);

                Console.ReadKey();
            }
        }
        public static void QueueConsumerLab(ConnectionFactory factory)
        {
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                string queue = "queueconsumerqueue";
                channel.QueueDeclare(queue, false, false, false, null);
                // Queue 不推荐使用方式（已过时）
                QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
                while (true)
                {
                    var result = consumer.Queue.Dequeue();
                    if (result == null)
                    {
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.WriteLine(Encoding.UTF8.GetString(result.Body));
                        channel.BasicAck(result.DeliveryTag, false);
                    }
                }
            }
        }
    }
}
