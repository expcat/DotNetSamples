using System;
using Mongo.Extension;
using MongoDB.Driver;

namespace Mongo.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MongoClient($"mongodb://{ConfigHelper.user}:{ConfigHelper.password}@{ConfigHelper.host}:{ConfigHelper.port}");
            var db = client.GetDatabase("testdb");
            // db.CreateCollection("test_collection");
            var collection = db.GetCollection<TestModel>("test_collection");
            collection.InsertOne(new TestModel { Id = 1, Name = "hello" });
            Console.WriteLine("添加成功！");
        }
    }

    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
