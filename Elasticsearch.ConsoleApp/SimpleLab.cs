using System;
using Elasticsearch.ConsoleApp.Models;
using Elasticsearch.Net;

namespace Elasticsearch.ConsoleApp
{
    public class SimpleLab
    {
        public static void IndexLab(ElasticLowLevelClient lowlevelClient)
        {
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
}
