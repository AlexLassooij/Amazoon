using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongoTest.Models
{
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string name { get; set; }
        
        public int weight { get; set; }

        public int volume { get; set; }

        public ItemState itemState { get; set; }

        public int warehouseID { get; set; }

    }
}