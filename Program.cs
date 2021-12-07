using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using mongoTest.Models;
using mongoTest.Components;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace mongoTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // brew services start mongodb/brew/mongodb-community
            // mongosh
            initializeDB();
            InitializeWarehouses();
            CreateHostBuilder(args).Build().Run();
            //IMongoCollection<BsonDocument> _items = ConnectionHelper.getItemCollection();
            //var newItem = new BsonDocument
            //{
            //    { "name", "cereal" },
            //    { "weight", "200" },
            //    { "volume", "300" },
            //    { "itemState", "available" }
            //};
            //_items.InsertOne(newItem);
            //List<BsonDocument> returnedItems = _items.Find(new BsonDocument()).ToList();
            //Console.WriteLine(returnedItems);


        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        // TODO : fix up jsondeserialization so that we don't need to use BsonDocument deserialization
        public static void initializeDB()
        {

            IMongoCollection<BsonDocument> _bson = ConnectionHelper.getBsonCollection();
            IMongoCollection<Item> _items = ConnectionHelper.getItemCollection();
            IMongoCollection<Order> _orders = ConnectionHelper.getOrderCollection();
            _items.DeleteMany(Item => true);
            _orders.DeleteMany(Order => true);

            string inputFileName = "initialItems.json"; // initialize to the input file to write to.

            using (var streamReader = new StreamReader(inputFileName))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    using (var jsonReader = new JsonReader(line))
                    {
                        var context = BsonDeserializationContext.CreateRoot(jsonReader);
                        var document = _bson.DocumentSerializer.Deserialize(context);

                        int amount = int.Parse(document.ElementAt(4).Value.ToString());

                        for (int i = 0; i < amount; i++)
                        {                            

                            Item newItem = new Item
                            {
                                name = document.ElementAt(0).Value.ToString(),
                                weight = int.Parse(document.ElementAt(1).Value.ToString()),
                                volume = int.Parse(document.ElementAt(2).Value.ToString()),
                                itemState = ItemState.Available,
                                warehouseID = int.Parse(document.ElementAt(3).Value.ToString()),


                            };
                            _items.InsertOne(newItem);
                        }
                        
                    }
                }
            }
        }

        public static void InitializeWarehouses()
        {
            Task t = Task.Run(() => new Warehouse(1, 4, 4, 3, 2, 2));
        }
    }
}
