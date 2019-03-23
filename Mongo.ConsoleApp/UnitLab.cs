using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.ConsoleApp
{
    public class UnitLab : IDisposable
    {
        IMongoDatabase _db;
        public UnitLab(IMongoDatabase db)
        {
            _db = db;
            DataInit();
        }

        void DataInit()
        {
            var collection = _db.GetCollection<Member>(typeof(Member).Name);
            collection.InsertOne(new Member { Id = ObjectId.GenerateNewId(), Name = "Jack", Male = true, CreateTime = DateTime.Now, Age = 12 });
            collection.InsertOne(new Member { Id = ObjectId.GenerateNewId(), Name = "Mary", Male = false, CreateTime = DateTime.Now, Age = 15 });
            collection.InsertOne(new Member { Id = ObjectId.GenerateNewId(), Name = "Tom", Male = true, CreateTime = DateTime.Now, Age = 18 });
            collection.InsertOne(new Member { Id = ObjectId.GenerateNewId(), Name = "Mike", Male = true, CreateTime = DateTime.Now, Age = 24 });
            Console.WriteLine("添加成功！");
        }

        void DataClear()
        {
            _db.GetCollection<Member>(typeof(Member).Name).DeleteMany(new BsonDocument());
            Console.WriteLine("删除成功！");
        }

        public void Dispose()
        {
            DataClear();
        }
    }
}
