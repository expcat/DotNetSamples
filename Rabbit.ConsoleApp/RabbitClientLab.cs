using System;
using System.Text;
using System.Threading;
using Rabbit.Extension;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.ConsoleApp
{
    public class RabbitClientLab
    {
        private static readonly string host = ConfigHelper.host;
        private static readonly int port = ConfigHelper.port;
        private static readonly string userName = ConfigHelper.user;
        private static readonly string password = ConfigHelper.password;
        public static void LabClientPublish()
        {
            //1.实例化连接工厂
            var factory = new ConnectionFactory()
            {
                HostName = host,
                Port = port,
                UserName = userName,
                Password = password
            };
            //2. 建立连接
            using(var connection = factory.CreateConnection())
            //3. 创建信道
            using(var channel = connection.CreateModel())
            {
                //4. 申明队列
                channel.QueueDeclare(queue: "hello", durable : false, exclusive : false, autoDelete : false, arguments : null);
                Console.WriteLine("Client is opened.");
                while (true)
                {
                    string message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message))
                        continue;
                    else if (message.Equals("exit", StringComparison.OrdinalIgnoreCase))
                        break;
                    //5. 构建byte消息数据包
                    var body = Encoding.UTF8.GetBytes(message);
                    //6. 发送数据包
                    channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties : null, body : body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
                Console.WriteLine("Client is closed.");
            }
        }

        public static void LabClientReceive()
        {
            //1.实例化连接工厂
            var factory = new ConnectionFactory()
            {
                HostName = host,
                Port = port,
                UserName = userName,
                Password = password
            };
            //2. 建立连接
            using(var connection = factory.CreateConnection())
            //3. 创建信道
            using(var channel = connection.CreateModel())
            {
                //4. 申明队列
                channel.QueueDeclare(queue: "hello", durable : false, exclusive : false, autoDelete : false, arguments : null);
                //5. 构造消费者实例
                var consumer = new EventingBasicConsumer(channel);
                //6. 绑定消息接收后的事件委托
                consumer.Received += (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                //7. 启动消费者
                channel.BasicConsume(queue: "hello", autoAck : true, consumer : consumer);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
