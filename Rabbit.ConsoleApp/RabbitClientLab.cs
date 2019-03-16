using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.ConsoleApp
{
    public class RabbitClientLab
    {
        public static void LabClientPublish(ConnectionFactory factory)
        {
            //1. 建立连接
            using(var connection = factory.CreateConnection())
            //2. 创建信道
            using(var channel = connection.CreateModel())
            {
                //3. 申明队列
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

        public static void LabClientReceive(ConnectionFactory factory)
        {
            //1. 建立连接
            using(var connection = factory.CreateConnection())
            //2. 创建信道
            using(var channel = connection.CreateModel())
            {
                //3. 申明队列
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
    }
}
