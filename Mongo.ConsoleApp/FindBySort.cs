using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.ConsoleApp
{
    public class FindBySort
    {
        public static async Task Descending(IMongoDatabase db)
        {
            Console.WriteLine("Find by descending:");
            var collection = db.GetCollection<Member>(typeof(Member).Name);
            var filter = Builders<Member>.Filter.Exists("Age");
            var sort = Builders<Member>.Sort.Descending("Age");
            await collection.Find(filter).Sort(sort).ForEachAsync(d => Console.WriteLine(d.ToJson()));
        }

        public static async Task Ascending(IMongoDatabase db)
        {
            Console.WriteLine("Find by ascending:");
            var collection = db.GetCollection<Member>(typeof(Member).Name);
            var filter = Builders<Member>.Filter.Exists("Age");
            var sort = Builders<Member>.Sort.Ascending("Age");
            await collection.Find(filter).Sort(sort).ForEachAsync(d => Console.WriteLine(d.ToJson()));
        }
    }
}
