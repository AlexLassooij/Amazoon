using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using mongoTest.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mongoTest.Controllers
{
    public class OrderController : Controller
    {
        IMongoCollection<Item> _items = ConnectionHelper.getItemCollection();
        // GET: /<controller>/
        public IActionResult Index()
        {            
            List<Item> itemsFromDB = _items.Find(Item => true).ToList();

            Dictionary<string, int> itemViewDict = new Dictionary<string, int>();
            List <ItemView> itemView = new List<ItemView>();

            foreach(Item item in itemsFromDB)
            {
                string name = item.name;
                if (!itemViewDict.ContainsKey(name))
                {
                    itemViewDict.Add(name, 1);
                } else
                {
                    itemViewDict[name] += 1;
                }
            }

            foreach(var entry in itemViewDict)
            {
                itemView.Add(new ItemView { name = entry.Key, amountAvailable = entry.Value });
            }

            return View(itemView);
        }

        [HttpPost]
        public IActionResult PlaceOrder(string customerName, string orderDetails)
        {
            IMongoCollection<Order> _orders = ConnectionHelper.getOrderCollection();
            List<string> parsedOrders = orderDetails.Split(',').ToList();
            List<Item> orderItems = new List<Item>();

            foreach(string pair in parsedOrders)
            {
                List<string> splitPair = pair.Split(' ').ToList();


                FilterDefinition<Item> filter = new BsonDocument
                {
                    { "name", splitPair[0] },
                    { "itemState", ItemState.Available }
                };


                UpdateDefinition<Item> update = Builders<Item>.Update.Set("itemState", ItemState.Purchased);
                //UpdateDefinition<Item> update = new BsonDocument
                //{
                //    { "itemState", "purchased" }
                //};           

                for (int i = 0; i < int.Parse(splitPair[1]); i++)
                {
                    Item item = _items.FindOneAndUpdate(filter, update);
                    orderItems.Add(item);
                }
            }

            Order newOrder = new Order {
                clientName = customerName,
                items = orderItems,
                orderState = OrderState.Placed };
            _orders.InsertOne(newOrder);

            ViewData["order"] = newOrder;
            return View();             
        }       
    }
}
