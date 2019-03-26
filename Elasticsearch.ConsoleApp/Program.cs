using System;
using Elasticsearch.Extension;
using Elasticsearch.Net;

namespace Elasticsearch.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new ConnectionConfiguration(new Uri($"http://{ConfigHelper.host}:{ConfigHelper.port}"))
                .RequestTimeout(TimeSpan.FromMinutes(2))
                .BasicAuthentication(ConfigHelper.user, ConfigHelper.password);
            var lowlevelClient = new ElasticLowLevelClient(settings);

            // lab 1 Simple Index
            SimpleLab.IndexLab(lowlevelClient);
        }
    }

}
