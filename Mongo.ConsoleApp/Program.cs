using System;
using Mongo.Extension;
using MongoDB.Bson;
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
            // InsertFindDelete.LabInsert(db);
            // InsertFindDelete.LabFind(db);
            // InsertFindDelete.LabDelete(db);

            // Lab 2 Find Advanced
            using(var unitLab = new UnitLab(db))
            {
                FindAdvancedLab.LabFindAll(db);
                FindAdvancedLab.LabFindByFilter(db);
            }
        }

    }
}
