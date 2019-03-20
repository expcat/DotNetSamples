using Microsoft.Extensions.Configuration;

namespace Mongo.Extension
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
            host = conf["mongo:host"];
            port = int.Parse(conf["mongo:port"]);
            user = conf["mongo:user"];
            password = conf["mongo:password"];
        }

    }
}
