using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongoTest.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("clientName")]
        public string clientName { get; set; }

        public List<Item> items { get; set; }

        public OrderState orderState { get; set; }
    }
}