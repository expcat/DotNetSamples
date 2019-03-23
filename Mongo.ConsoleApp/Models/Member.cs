using System;
using MongoDB.Bson;

namespace Mongo.ConsoleApp
{
    public class Member
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public bool Male { get; set; }
        public DateTime CreateTime { get; set; }
        public int Age { get; set; }
    }
}
