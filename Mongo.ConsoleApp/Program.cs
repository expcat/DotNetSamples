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

            // lab 1 Insert & Get & Delete
            InsertGetDelete.LabInsert(db);
            InsertGetDelete.LabGet(db);
            InsertGetDelete.LabDelete(db);
        }
    }
}
