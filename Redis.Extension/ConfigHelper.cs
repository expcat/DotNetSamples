using Microsoft.Extensions.Configuration;

namespace Redis.Extension
{
    public class ConfigHelper
    {
        public static readonly string host;
        public static readonly string password;
        static ConfigHelper()
        {
            var conf = new ConfigurationBuilder().AddJsonFile("secret.json").Build();
            host = conf["redis:host"];
            password = conf["redis:password"];
        }

    }
}
