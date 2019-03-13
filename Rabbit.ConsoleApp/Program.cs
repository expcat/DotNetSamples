using System;

namespace Rabbit.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // lab 1 RabbitClient
            if (args.Length > 0 && args[0].Equals("client", StringComparison.OrdinalIgnoreCase))
                RabbitClientLab.LabClientPublish();
            else
                RabbitClientLab.LabClientReceive();
        }
    }
}
