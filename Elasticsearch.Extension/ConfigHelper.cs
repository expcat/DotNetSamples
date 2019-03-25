using Microsoft.Extensions.Configuration;

namespace Elasticsearch.Extension
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
            host = conf["elasticsearch:host"];
            port = int.Parse(conf["elasticsearch:port"]);
            user = conf["elasticsearch:user"];
            password = conf["elasticsearch:password"];
        }

    }
}
