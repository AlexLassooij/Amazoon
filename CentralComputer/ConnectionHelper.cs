using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using mongoTest.Models;
using System.Linq;


namespace mongoTest
{
    public static class ConnectionHelper
    {
        public static IMongoCollection<Item> getItemCollection()
        {
            // string connectionString = configuration.GetConnectionString("DefaultConnectionString");
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("Amazon");
            return database.GetCollection<Item>("Items");
        }

        public static IMongoCollection<Order> getOrderCollection()
        {
            // string connectionString = configuration.GetConnectionString("DefaultConnectionString");
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("Amazon");
            return database.GetCollection<Order>("Orders");
        }

        public static IMongoCollection<BsonDocument> getBsonCollection()
        {
            // string connectionString = configuration.GetConnectionString("DefaultConnectionString");
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("Amazon");
            return database.GetCollection<BsonDocument>("Items");
        }


    }
}
