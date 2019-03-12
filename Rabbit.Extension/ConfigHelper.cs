using Microsoft.Extensions.Configuration;

namespace Rabbit.Extension
{
    public class ConfigHelper
    {
        public static readonly string host;
        public static readonly int port;
        public static readonly string user;
        public static readonly string password;
        static ConfigHelper()
        {
            var conf = new ConfigurationBuilder().AddJsonFile("secret.json").Build();
            host = conf["rabbit:host"];
            port = int.Parse(conf["rabbit:port"]);
            user = conf["rabbit:user"];
            password = conf["rabbit:password"];
        }

    }
}
