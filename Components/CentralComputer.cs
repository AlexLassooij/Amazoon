using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using mongoTest.Models;
namespace mongoTest.Components
{
    public class CentralComputer
    {
        public static int DEFAULT_RESTOCK_AMOUNT = 10;
        public static int MIN_AMOUNT_NEEDED = 5;
        private Warehouse currentWarehouse;
        private List<Item> inventory = new List<Item>();
        private List<ItemLocation> inventoryLocations = new List<ItemLocation>();
        private List<Item> restockItems = new List<Item>();

        // queue should probably be moved to warehouse
        private Queue<RobotTask> robotTaskQueue = new Queue<RobotTask>();
        private Queue<Order> orderQueue;
        

        private Mutex robotQueueMutex = new Mutex();
        public Mutex dockMutex = new Mutex();
        private Semaphore truckSemaphore;
        private Task[] robotTasks;
        private IMongoCollection<Item> _items = ConnectionHelper.getItemCollection();
        private IMongoCollection<Order> _orders = ConnectionHelper.getOrderCollection();



       



        // TODO :
        // Truck communication with CC
        // finish up robots : continuous loop for robot activity
        // 
        
        // Notes on webserver :
        // 
        

        public CentralComputer(Warehouse currentWarehouse)
        {
            this.currentWarehouse = currentWarehouse;
            truckSemaphore = new Semaphore(0, currentWarehouse.getNumDocks());
            robotTasks = new Task[currentWarehouse.numRobots];
            currentWarehouse.SetComputer(this);
            RunWarehouse();
        }

        //////////////////////////////////////////////////////////////////////// Central Computer & Truck interaction ////////////////////////////////////////////////////////////////////////
        // 
        // a central computer should be able to alert trucks of empty dock spots
        // a truck must be able to notify central computer of its arrival & departure

        // Plan:
        // Make a Dock class
        // Dock Attributes:
        // + DockID: int
        // + positionX: int
        // + DockStatus: DockState
        //
        // DockingStation Methods:
        // ===> N/A
        // Docking Station status will be updated via central computer
        // because the trucks will be interacting with the central computer

        int numberOfDockingStations = 2; // just for testing. this number should be imported from when warehouse gets constructed
        private Dock[] dockList = new Dock[2]; // for now two docks

        private void getDockingStationStatus(int DockID)
        {
            // this functino returns the status of every docking station
            foreach (Dock dock in dockList)
            {
                if (dock.DockID == DockID)
                {
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                    Console.WriteLine($"Dock ID: {dock.DockID}             $$$");
                    Console.WriteLine($"Dock Position: {dock.positionX}    $$$");
                    Console.WriteLine($"Dock Status: {dock.dockState}     $$$");
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                    Console.WriteLine("");
                }
            }
        }
        private void updateDockingStationStatus(int DockID, DockState dockState)
        {
            foreach (Dock dock in dockList)
            {
                if (dock.DockID == DockID)
                {
                    dock.setDockState(dockState);
                    Console.WriteLine($"Updated Dock Status for {dock.DockID}: ");
                    getDockingStationStatus(dock.DockID);
                }
                else
                    Console.WriteLine($"Failed to find Dock {dock.DockID}");
            }
        }
        // call TRUNCATE TABLE tableName for each table
        //public void initializeComputer()
        //{
        //    this.currentWarehouse = new Warehouse(
        //        this, 4, 4, 2);                                
        //    for (int i = 0; i < this.numRobots; i++)
        //    {

        //        robots[i] = new Robot();
        //        var robot = robots[i];                
        //    }
        //}       

        // central function of the computer, will always run
        public void RunWarehouse()
        {
            PerformInitialRestock();
            InitRobots();
            InitDocks();
            CreateShippingTruck();
            // this will make sure the truck is docked at an open dock
            //var isTruckDocked = Task<bool>.Run(() =>
            //{
            //    restockTruck.runTruck();
            //    });
 


            while (true)
           {
                pollForNewOrders();
                CheckLowStockItems();

                Console.WriteLine("Central computer running");
                Thread.Sleep(1000);
           }
        }

        private void CheckLowStockItems()
        {
            Dictionary<string, int> itemAmountDictioanry = new Dictionary<string, int>();

            // checks how many of each item are in stock
            foreach (Item item in inventory)
            {
                string name = item.name;
                if (item.itemState == ItemState.Available || item.itemState == ItemState.Incoming)
                {
                    if (!itemAmountDictioanry.ContainsKey(name))
                    {
                        itemAmountDictioanry.Add(name, 1);
                    }
                    else
                    {
                        itemAmountDictioanry[name] += 1;
                    }
                }
            }

            // checks if any items in the warehouse are low in stock
            foreach (var dictEntry in itemAmountDictioanry)
            {
                if (dictEntry.Value < MIN_AMOUNT_NEEDED)
                {
                    // adds items to a restock list
                    AddItemsToRestock(dictEntry.Key);
                }
            }
        }

        // adds the required items to the item to restock list

        public void AddItemsToRestock(string itemName)
        {
            int[] itemattributes = ItemAttribute.GetItemAttributes(itemName);
            int weight = itemattributes[0];
            int volume = itemattributes[1];

            for (int i = 0; i < DEFAULT_RESTOCK_AMOUNT; i++)
            {
                Item newItem = new Item
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    name = itemName,
                    weight = weight,
                    volume = volume,
                    itemState = ItemState.Incoming,
                    warehouseID = currentWarehouse.getID(),
                };

                
                if ((getTotalItemWeight(restockItems) + weight) <= Truck.MAX_WEIGHT && (getTotalItemVolume(restockItems) + volume) <= Truck.MAX_VOLUME)
                {
                    // keeps adding items to restock list until a single truck can no longer
                    // fit all items
                    restockItems.Add(newItem);
                } else
                {
                    // if truck is full, dispatches a restocking truck,
                    // then keeps adding the rest (if any left) of the items to
                    // the list for another truck to bring
                    Console.WriteLine($"Dispatching a restocking truck with total weight : {getTotalItemWeight(restockItems)} and volume : {getTotalItemVolume(restockItems)} and the following items: " + restockItems.ToString());
                    CreateRestockingTruck(restockItems);
                    UpdateInventory(restockItems);
                    restockItems.Clear();
                }
            }
        }

        public void CreateShippingTruck()
        {
            ShippingTruck ShippingTruck = new ShippingTruck(currentWarehouse, 0, 8);
            currentWarehouse.AddShippingTruck(ShippingTruck);
            Task.Run(() => ShippingTruck.RunTruck());
        }

        private void CreateRestockingTruck(List<Item> restockItems)
        {
            int defaultX = 0;
            int defaultY = 8;

            RestockTruck restockTruck = new RestockTruck(currentWarehouse, defaultX, defaultY, restockItems);
            currentWarehouse.AddRestockTruck(restockTruck);

            
            Task.Run(() => restockTruck.RunTruck());
            
        }        

        private void InitRobots() {
            for (int i = 0; i <= currentWarehouse.getWarehouseColumns(); i++) {
                currentWarehouse.getRobots()[i] = new Robot(this, robotQueueMutex, i);
                Robot newRobot = currentWarehouse.getRobots()[i];
                Task t = Task.Run(() => newRobot.RunRobot());
                // robotTasks[i] = t;
            }
        }

        private void InitDocks()
        {
            // position Y of all docks will be below the bottom row of the warehouse
            for (int i = 0; i < currentWarehouse.getNumDocks(); i++ )
            {
                currentWarehouse.getDocks()[i] = new Dock(i + 1, i, currentWarehouse.getWarehouseRows(), DockState.Available);
            }
        }

        
       //restocking
        public void CreateUnloadTask(List<Item> restockingOrder, Truck restockTruck)
        {
            QueueRobotTasks("unload", restockingOrder, restockTruck);
        }


        private void pollForNewOrders()
        {
            Console.WriteLine("polling for new orders");

            List<Order> orders = _orders.Find(Order => true).ToList();
            List<Item> itemsInWarehouse = new List<Item>();


            foreach (Order order in orders)
            {
                // find all the items in this order that are available in this
                // warehouse and have not been scheduled for loading
                itemsInWarehouse = GetItemsInWarehouse(order);
                // only schedules task if the order contains any items that have not been
                // been schedules for loading yet
                if (itemsInWarehouse.Count > 0)
                {
                    ShippingTruck ShippingTruck = GetShippingTruck(getTotalItemWeight(itemsInWarehouse), getTotalItemVolume(itemsInWarehouse));
                    // only schedules a task if there is a truck available to pick
                    // up the items 
                    if (ShippingTruck != null)
                    {
                        ShippingTruck.addWeight(getTotalItemWeight(itemsInWarehouse));
                        ShippingTruck.addVolume(getTotalItemVolume(itemsInWarehouse));
                        CreateLoadTask(itemsInWarehouse, ShippingTruck);
                    }
                }
            }
        }

        // updates the database to reflect that items are going to be picked up
        // for Shipping
        // adds a task to the queue to have robots load the items
        private void CreateLoadTask(List<Item> itemsInWarehouse, Truck ShippingTruck)
        {
            // list of items in this order that are present in current warehouse
            FilterDefinition<Item> filter = new BsonDocument
                {
                    { "warehouseID", currentWarehouse.getID() },
                    { "itemState", ItemState.Purchased }
                };

            UpdateDefinition<Item> update = Builders<Item>.Update.Set("itemState", ItemState.Loading);

            foreach (Item item in itemsInWarehouse)
            {                
                _items.FindOneAndUpdate(filter, update);
                item.itemState = ItemState.Loading;                
            }

            QueueRobotTasks("load", itemsInWarehouse, ShippingTruck);            
        }

        private List<Item> GetItemsInWarehouse(Order order)
        {
            List<Item> itemsInWarehouse = new List<Item>();

            foreach (Item item in order.items)
            {
                if (item.warehouseID == currentWarehouse.getID() && item.itemState == ItemState.Purchased)
                {                    
                    itemsInWarehouse.Add(item);
                }
            }

            return itemsInWarehouse;
        }

        // queues robot tasks for a particular Order
        // this will happen after a new order comes in and there is a truck
        // available to ship this order
        // 
        private void QueueRobotTasks(string taskType, List<Item> items, Truck truck)
        {
            List<Item> itemsList = new List<Item>();

            if (taskType == "load")
            {
                foreach (Item item in items)
                {
                    if (getTotalItemWeight(itemsList) + item.weight < Robot.GetMaxWeight())
                    {
                        itemsList.Add(item);
                    }
                    else
                    {
                        // no need for mutex, since only one computer will modify it
                        robotQueueMutex.WaitOne();
                        robotTaskQueue.Enqueue(new RobotTask(taskType, itemsList, truck));
                        robotQueueMutex.ReleaseMutex();
                        itemsList.Clear();
                    }
                }
            }
            else if (taskType == "unload")
            {
                int itemTotalWeight = 0;
                foreach (Item item in items)
                {
                    while (itemTotalWeight < Robot.GetMaxWeight())
                    {
                        itemsList.Add(item);
                        itemTotalWeight += item.weight;
                    }
                    robotTaskQueue.Enqueue(new RobotTask(taskType, itemsList, truck));
                    itemsList.Clear();
                }
            }

            robotTaskQueue.Enqueue(new RobotTask(taskType, itemsList, truck));
        }

        // will only be called at start to magically add all items to warehouse
        private void PerformInitialRestock()
        {
            List<Item> items = _items.Find(item => item.warehouseID == currentWarehouse.getID()).ToList();
            foreach (Item item in items)
            {
                ItemLocation location = GenerateRandomLocation(item);
                location.items.Add(item.Id);
                location.currentWeight += item.weight;
                inventoryLocations.Add(location);
                inventory.Add(item);
            }
        }

        // adds items on an incoming restock truck to the warehouse's inventory and pre designates a location for them
        // updates the database as well
        // items will be added to inventory but will have "Incoming" status
        // when robots restock items from truck, all the would need to do is
        // change status to available
        private void UpdateInventory(List<Item> restockItems)
        {
            foreach (Item item in restockItems)
            {
                ItemLocation location = GenerateRandomLocation(item);
                location.items.Add(item.Id);
                location.currentWeight += item.weight;
                inventoryLocations.Add(location);
                inventory.Add(item);
            }
            AddItemsToDatabase(restockItems);
        }

        private void AddItemsToDatabase(List<Item> items)
        {            
                _items.InsertMany(items);            
        }

        private ItemLocation GenerateRandomLocation(Item item)
        {
            Random rand = new Random();
            int row = rand.Next(0, currentWarehouse.getWarehouseRows() + 1);
            int column = rand.Next(0, currentWarehouse.getWarehouseColumns() + 1);
            int shelf = rand.Next(0, currentWarehouse.getShelfHeight() + 1);
            string orientation = new List<string>() { "right", "left" }[rand.Next(0,2)];

            ItemLocation location =  new ItemLocation(row, column, shelf, orientation);
            if (!WillItemFitOnShelf(item, location))
            {
                location = GenerateRandomLocation(item);
            }

            return location;
        }

        private bool WillItemFitOnShelf(Item item, ItemLocation location)
        {
            return item.weight + location.currentWeight <= Warehouse.MAX_SHELF_WEIGHT;
        }

        public int getTotalItemWeight(List<Item> items)
        {
            int sum = 0;
            foreach (Item item in items)
            {
                sum += item.weight;
            }

            return sum;
        }

        public int getTotalItemVolume(List<Item> items)
        {
            int sum = 0;
            foreach (Item item in items)
            {
                sum += item.volume;
            }

            return sum;
        }

        // will run as its own thread
        // numTasks are the number of robot tasks that have been created
        // for the particular operation (loading an order, unloading a truck)
        // this is because we would not want to attempt to consume every
        // single task that is sitting in the queue all at once
        //private void consumeRobotTasks(int numTasks)
        //{
        //    List<Task> tasks = new List<Task>();
        //    for (int i = 0; i < numTasks; i++)
        //    {
        //        robotSemaphore.WaitOne();
        //        Robot robot = findAvailableRobot();
        //        Task t = Task.Run(() => consumeRobotTask(robot, robotTaskQueue.Dequeue()));
        //        tasks.Add(t);
        //    }

        //}
        

        // will be called before enqueueing a new robot tasks
        public ShippingTruck GetShippingTruck(int orderWeight, int orderVolume)
        {
            foreach (ShippingTruck truck in currentWarehouse.GetShippingTrucks())
            {
                if (truck.GetTruckState() == TruckState.Loading)
                {
                    if (orderWeight <= truck.getAvailableWeight() && orderVolume <= truck.getAvailableVolume())
                    {
                        return truck;
                    }
                }
            }
            return null;
        }

        // probably not needed
        public RestockTruck IsRestockTruckAvailable()
        {
            foreach (RestockTruck truck in currentWarehouse.GetRestockTrucks())
            {
                if (truck.GetTruckState() == TruckState.Docked)
                {
                    return truck;
                }
            }
            return null;
        }
                
        public Queue<RobotTask> GetRobotTasks() {
            return robotTaskQueue;
        }

        public Warehouse GetWarehouse()
        {
            return currentWarehouse;
        }

        public List<ItemLocation> GetInventoryLocations()
        {
            return inventoryLocations;
        }

        // have a polling method, checks for new orders every few seconds
        // if new order exists : add new pick up tasks for robots

        //public void addItem(Item item)
        //{
        //    Item item = 
        //}

        //public Item[] Append(Item[] items, Item item)
        //{
        //    if (items == null)
        //    {
        //        return new Item[] { item };
        //    }
        //    Item[] result = new Item[items.Length + 1];
        //    items.CopyTo(result, 0);
        //    result[items.Length] = item;
        //    return result;
        //}
    }
}
