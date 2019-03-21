using System;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Mongo.ConsoleApp
{
    public class InsertGetDelete
    {
        public static void LabInsert(IMongoDatabase db)
        {
            var collection = db.GetCollection<Member>(typeof(Member).Name);
            collection.InsertOne(new Member { Id = 1, Name = "Jack", Male = true, Brithday = DateTime.Now });
            Console.WriteLine("添加成功！");
        }

        public static void LabGet(IMongoDatabase db)
        {
            var collection = db.GetCollection<Member>(typeof(Member).Name);
            var result = collection.Find(m => m.Name == "Jack").FirstOrDefault();
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }

        public static void LabDelete(IMongoDatabase db)
        {
            var collection = db.GetCollection<Member>(typeof(Member).Name);
            var result = collection.DeleteOne(m => m.Id == 1);
            Console.WriteLine($"删除 {result.DeletedCount} 条");
        }
    }
}
