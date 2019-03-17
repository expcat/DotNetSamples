using System;
using System.Linq;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.ConsoleApp
{
    public class RabbitClientLab
    {
        private readonly static string[] level = new string[] { "debug", "info", "warning" };
        public static void LabClientPublish(ConnectionFactory factory)
        {
            //1. 建立连接
            using(var connection = factory.CreateConnection())
            //2. 创建信道
            using(var channel = connection.CreateModel())
            {
                //3. 声明队列(生产环境不再此声明)
                channel.QueueDeclare(queue: "hello", durable : false, exclusive : false, autoDelete : false, arguments : null);
                Console.WriteLine("Client is opened.");
                while (true)
                {
                    string message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message))
                        continue;
                    else if (message.Equals("exit", StringComparison.OrdinalIgnoreCase))
                        break;
                    //4. 构建byte消息数据包
                    var body = Encoding.UTF8.GetBytes(message);
                    //5. 发送数据包
                    channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties : null, body : body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
                Console.WriteLine("Client is closed.");
            }
        }

        public static void LabClientPublishWithBind(ConnectionFactory factory)
        {
            //1. 建立连接
            using(var connection = factory.CreateConnection())
            //2. 创建信道
            using(var channel = connection.CreateModel())
            {
                //3.1 声明 Exchange
                string exchange = "direct";
                channel.ExchangeDeclare(exchange, ExchangeType.Direct, false, false, null);

                //3.2 声明队列
                // string queue = "log_else";
                // channel.QueueDeclare(queue, false, false, false, null);

                // channel.QueueBind(queue, exchange, null, null);

                while (true)
                {
                    string message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message))
                        continue;
                    else if (message.Equals("exit", StringComparison.OrdinalIgnoreCase))
                        break;
                    string routingKey = string.Empty;
                    int i = message.IndexOf(':');
                    if (i > 0)
                    {
                        var arr = message.AsSpan();
                        routingKey = arr.Slice(0, i).ToString();
                        if (!level.Contains(routingKey))
                            routingKey = "error";
                        if (arr.Length - 1 == i)
                            message = "nil";
                        else
                            message = arr.Slice(i + 1).ToString();
                    }
                    else
                    {
                        routingKey = "info";
                        if (i == 0)
                        {
                            if (message.Length == 1)
                                message = "nil";
                            else
                                message = message.Substring(1);
                        }
                    }
                    //4. 构建byte消息数据包
                    var body = Encoding.UTF8.GetBytes(message);
                    //5. 发送数据包
                    channel.BasicPublish(exchange, routingKey, null, body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }

        public static void LabClientReceive(ConnectionFactory factory)
        {
            //1. 建立连接
            using(var connection = factory.CreateConnection())
            //2. 创建信道
            using(var channel = connection.CreateModel())
            {
                //3. 声明队列
                channel.QueueDeclare(queue: "hello", durable : false, exclusive : false, autoDelete : false, arguments : null);
                //4. 构造消费者实例
                var consumer = new EventingBasicConsumer(channel);
                //5. 绑定消息接收后的事件委托
                consumer.Received += (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                //6. 启动消费者
                channel.BasicConsume(queue: "hello", autoAck : true, consumer : consumer);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        public static void LabClientReceiveWithBindElse(ConnectionFactory factory)
        {
            //1. 建立连接
            using(var connection = factory.CreateConnection())
            //2. 创建信道
            using(var channel = connection.CreateModel())
            {
                //3.1 声明 Exchange
                string exchange = "myexchange";
                channel.ExchangeDeclare(exchange, ExchangeType.Direct, false, false, null);

                //3.2 声明队列
                string queue = "log_else";
                channel.QueueDeclare(queue, false, false, false, null);

                //3.3 绑定队列
                foreach (var item in level)
                {
                    channel.QueueBind(queue, exchange, item, null);
                }

                //4. 构造消费者实例
                var consumer = new EventingBasicConsumer(channel);
                //5. 绑定消息接收后的事件委托
                consumer.Received += (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    Console.WriteLine("{0}:{1}", ea.RoutingKey, message);
                };
                //6. 启动消费者
                channel.BasicConsume(queue, true, consumer);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        public static void LabClientReceiveWithBindError(ConnectionFactory factory)
        {
            //1. 建立连接
            using(var connection = factory.CreateConnection())
            //2. 创建信道
            using(var channel = connection.CreateModel())
            {
                //3.1 声明 Exchange
                string exchange = "myexchange";
                channel.ExchangeDeclare(exchange, ExchangeType.Direct, false, false, null);

                //3.2 声明队列
                string queue = "log_error";
                channel.QueueDeclare(queue, false, false, false, null);

                //3.3 绑定队列
                channel.QueueBind(queue, exchange, "error", null);

                //4. 构造消费者实例
                var consumer = new EventingBasicConsumer(channel);
                //5. 绑定消息接收后的事件委托
                consumer.Received += (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    Console.WriteLine("{0}:{1}", ea.RoutingKey, message);
                };
                //6. 启动消费者
                channel.BasicConsume(queue, true, consumer);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
