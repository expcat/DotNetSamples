using System;
using Rabbit.Extension;
using RabbitMQ.Client;

namespace Rabbit.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 实例化连接工厂
            var factory = new ConnectionFactory()
            {
                HostName = ConfigHelper.host,
                Port = ConfigHelper.port,
                UserName = ConfigHelper.user,
                Password = ConfigHelper.password
            };

            // lab 1 RabbitClient
            // if (args.Length > 0 && args[0].Equals("publish", StringComparison.OrdinalIgnoreCase))
            //     RabbitClientLab.LabClientPublish(factory);
            // else
            //     RabbitClientLab.LabClientReceive(factory);

            //lab 2 相同 Exchange 不同 RoutingKey 不同队列
            if (args.Length > 0)
            {
                if (args[0].Equals("else", StringComparison.OrdinalIgnoreCase))
                {
                    RabbitClientLab.LabClientReceiveWithBindElse(factory);
                }
                else if (args[0].Equals("error", StringComparison.OrdinalIgnoreCase))
                {
                    RabbitClientLab.LabClientReceiveWithBindError(factory);
                }
            }
            else
            {
                RabbitClientLab.LabClientPublishWithBind(factory);
            }
        }
    }
}
