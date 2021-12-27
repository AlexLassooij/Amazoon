using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongoTest.Models
{
    public class ItemView
    {
        
        public string name { get; set; }

        public int amountAvailable { get; set; }
    }
}