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
            if (args.Length > 0 && args[0].Equals("client", StringComparison.OrdinalIgnoreCase))
                RabbitClientLab.LabClientPublish(factory);
            else
                RabbitClientLab.LabClientReceive(factory);

        }
    }
}
