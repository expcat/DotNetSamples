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

            var person = new Person
            {
                Id = 1, Name = "Jack"
            };
            var indexResponse = lowlevelClient.Index<BytesResponse>("people", "person", "1", PostData.Serializable(person));
            byte[] responseBytes = indexResponse.Body;

            var asyncIndexResponse = lowlevelClient.IndexAsync<StringResponse>("people", "person", "1", PostData.Serializable(person)).GetAwaiter().GetResult();
            string responseString = asyncIndexResponse.Body;

            Console.WriteLine(responseString);
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
