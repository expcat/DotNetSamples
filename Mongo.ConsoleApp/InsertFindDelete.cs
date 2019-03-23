using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.ConsoleApp
{
    public class InsertFindDelete
    {
        private static ObjectId id;
        public static void LabInsert(IMongoDatabase db)
        {
            var collection = db.GetCollection<Member>(typeof(Member).Name);
            id = ObjectId.GenerateNewId();
            collection.InsertOne(new Member { Id = id, Name = "Jack", Male = true, CreateTime = DateTime.Now, Age = 12 });
            Console.WriteLine("添加成功！");
        }

        public static void LabFind(IMongoDatabase db)
        {
            var collection = db.GetCollection<Member>(typeof(Member).Name);
            Console.WriteLine(collection.Find(m => m.Name == "Jack").FirstOrDefault().ToJson());
        }

        public static void LabDelete(IMongoDatabase db)
        {
            var collection = db.GetCollection<Member>(typeof(Member).Name);
            var result = collection.DeleteOne(m => m.Id == id);
            Console.WriteLine($"删除 {result.DeletedCount} 条");
        }
    }
}
