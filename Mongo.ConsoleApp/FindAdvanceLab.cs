using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;

namespace Mongo.ConsoleApp
{
    public class FindAdvancedLab
    {
        public static void LabFindAll(IMongoDatabase db)
        {
            Console.WriteLine("Find All:");
            var collection = db.GetCollection<Member>(typeof(Member).Name);
            Console.WriteLine(collection.CountDocuments(Builders<Member>.Filter.Empty) + "个Document");
            // or Async
            Console.WriteLine("Async:");
            collection.Find(new BsonDocument()).ForEachAsync(d => Console.WriteLine(d.ToJson())).GetAwaiter().GetResult();
            // Sync
            Console.WriteLine("Sync:");
            var cursor = collection.Find(new BsonDocument()).ToCursor();
            foreach (var d in cursor.ToEnumerable())
            {
                Console.WriteLine(d.ToJson());
            }
        }

        public static void LabFindByFilter(IMongoDatabase db)
        {
            Console.WriteLine("Find By Filter:");
            var maleFilter = Builders<Member>.Filter.Eq("Male", false);
            var collection = db.GetCollection<Member>(typeof(Member).Name);
            var document = collection.Find(maleFilter).FirstOrDefault();
            Console.WriteLine(document.ToJson());

            var filterBuilder = Builders<Member>.Filter;
            var ageFilter = filterBuilder.Gt("Age", 13) & filterBuilder.Lte("Age", 18);
            collection.Find(ageFilter).ForEachAsync(d => Console.WriteLine(d.ToJson()));
        }
    }
}
