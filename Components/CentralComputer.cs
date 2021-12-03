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
        private Warehouse currentWarehouse;
        private List<Item> inventory = new List<Item>();
        private List<ItemLocation> inventoryLocations = new List<ItemLocation>();

        // queue should probably be moved to warehouse
        private Queue<RobotTask> robotTaskQueue = new Queue<RobotTask>();
        private Queue<Order> orderQueue;

        private Mutex robotQueueMutex = new Mutex();
        private Mutex dockMutex = new Mutex();
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
            runWarehouse();
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

        public Warehouse getWarehouse()
        {
            return this.currentWarehouse;
        }

        // central function of the computer, will always run
        public void runWarehouse()
        {
            performInitialRestock();
            DeliveryTruck deliveryTruck = new DeliveryTruck(currentWarehouse, 0, 8);
            currentWarehouse.addTruck(deliveryTruck);
            Task.Run(() => new DeliveryTruck(currentWarehouse, 0, 8).runTruck());
            initRobots();
            initDocks();

           while (true)
           {
                pollForNewOrders();

                Console.WriteLine("Central computer running");
                Thread.Sleep(1000);
           }
        }

        private void initRobots() {
            for (int i = 0; i < currentWarehouse.numRobots; i++) {
                currentWarehouse.getRobots()[i] = new Robot(this, robotQueueMutex);
                Robot newRobot = currentWarehouse.getRobots()[i];
                Task t = Task.Run(() => newRobot.runRobot());
                // robotTasks[i] = t;
            }
        }

        private void initDocks()
        {
            // position Y of all docks will be below the bottom row of the warehouse
            for (int i = 0; i < currentWarehouse.getNumDocks(); i++ )
            {
                currentWarehouse.getDocks()[i] = new Dock(i + 1, i, currentWarehouse.getWarehouseRows(), DockState.Available);
            }
        }

        private void pollForNewOrders()
        {
            Console.WriteLine("polling for new orders");

            List<Order> orders = _orders.Find(Order => true).ToList();
            List<Item> itemsInWarehouse = new List<Item>();

            
            foreach(Order order in orders)
            {
                // find all the items in this order that are available in this
                // warehouse and have not been scheduled for loading
                itemsInWarehouse = getItemsInWarehouse(order);
                // only schedules task if the order contains any items that have not been
                // been schedules for loading yet
                if (itemsInWarehouse.Count > 0)
                {
                    DeliveryTruck deliveryTruck = isTruckAvailable(getTotalItemWeight(itemsInWarehouse), getTotalItemVolume(itemsInWarehouse));
                    // only schedules a task if there is a truck available to pick
                    // up the items 
                    if (deliveryTruck != null)
                    {
                        deliveryTruck.addWeight(getTotalItemWeight(itemsInWarehouse));
                        deliveryTruck.addVolume(getTotalItemVolume(itemsInWarehouse));
                        createLoadTask(itemsInWarehouse, deliveryTruck);
                    }
                }                                
            }
        }

        // updates the database to reflect that items are going to be picked up
        // for delivery
        // adds a task to the queue to have robots load the items
        private void createLoadTask(List<Item> itemsInWarehouse, Truck deliveryTruck)
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

            queueRobotTasks("load", itemsInWarehouse, deliveryTruck);            
        }

        private List<Item> getItemsInWarehouse(Order order)
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
        private void queueRobotTasks(string taskType, List<Item> items, Truck truck)
        {
            int numTasks = 0;
            List<Item> itemsList = new List<Item>();
            foreach (Item item in items)
            {
                if (getTotalItemWeight(itemsList) + item.weight < Robot.getMaxWeight())
                {
                    itemsList.Add(item);
                }
                else
                {
                    // no need for mutex, since only one computer will modify it
                    robotTaskQueue.Enqueue(new RobotTask(taskType, itemsList, truck));
                    itemsList.Clear();
                    numTasks += 1;
                }
            }
            robotTaskQueue.Enqueue(new RobotTask(taskType, itemsList, truck));
        }

        // will only be called at start to magically add all items to warehouse
        private void performInitialRestock()
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

        private void addItemsToDatabase(List<Item> items)
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
            if (!willItemFitOnShelf(item, location))
            {
                location = GenerateRandomLocation(item);
            }

            return location;
        }

        private bool willItemFitOnShelf(Item item, ItemLocation location)
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
        public DeliveryTruck isTruckAvailable(int orderWeight, int orderVolume)
        {
            foreach (DeliveryTruck truck in currentWarehouse.getTrucks())
            {
                if (truck.getTruckState() == TruckState.Docked)
                {
                    if (orderWeight <= truck.getAvailableWeight() && orderVolume <= truck.getAvailableVolume())
                    {
                        return truck;
                    }
                }
            }
            return null;
        }
                
        public Queue<RobotTask> GetRobotTasks() {
            return robotTaskQueue;
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
