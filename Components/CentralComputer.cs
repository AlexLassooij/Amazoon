using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Driver;
using mongoTest.Models;

namespace mongoTest.Components
{
    public class CentralComputer
    {
        private Warehouse currentWarehouse;
        private List<Item> inventory;
        private ItemLocation[] inventoryLocations { get; }

        // queue should probably be moved to warehouse
        private Queue<RobotTask> robotTaskQueue;
        private Queue<Order> orderQueue;

        private Semaphore robotSemaphore;
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
            robotSemaphore = new Semaphore(currentWarehouse.numRobots, currentWarehouse.numRobots);
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
           // dummy variable for now
           bool newOrderCameIn = false;

           while (true)
           {
                //if (newOrderCameIn)
                //{
                //    handleNewOrder(newOrder);
                //}

                Console.WriteLine("Central computer running");

               Thread.Sleep(1000);
           }
        }

        private void initRobots() {
            for (int i = 0; i < currentWarehouse.numRobots; i++) {
                currentWarehouse.getRobots()[i] = new Robot(this);
                Task t = Task.Run(() => currentWarehouse.getRobots()[i].runRobot());
                robotTasks[i] = t;
            }
        }

        private void pollForNewOrder()
        {

        }

        private void handleNewOrder(Order order)
        {
            List<Item> items = order.items;
            Truck loadingTruck = isTruckAvailable(getTotalItemWeight(items), getTotalItemVolume(items));
            if (loadingTruck != null)
            {
                int numTasks = queueRobotTasks("load", items, loadingTruck);
                // Task.Run(() => consumeRobotTasks(numTasks));
            }
        }

        // queues robot tasks for a particular Order
        // this will happen after a new order comes in and there is a truck
        // available to ship this order
        private int queueRobotTasks(string taskType, List<Item> items, Truck truck)
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
            return numTasks;
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
        public Truck isTruckAvailable(int orderWeight, int orderVolume)
        {
            foreach (Truck truck in currentWarehouse.getTrucks())
            {
                if (truck.getTruckState() == TruckState.Loading)
                {
                    if (orderWeight <= truck.getAvailableWeight() && orderVolume <= truck.getAvailableVolume())
                    {
                        return truck;
                    }
                }
            }
            return null;
        }

        public Dock getAvailableStation() {
            foreach(Dock dock in currentWarehouse.getDocks()) {
                if (dock.isAvailable()) {
                    return dock;
                }                
            }
            return null;
        }

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

        public Queue<RobotTask> GetRobotTasks() {
            return robotTaskQueue;
        }

        // have a polling method, checks for new orders every few seconds
        // if new order exists : add new pick up tasks for robots

        //public void addItem(Item item)
        //{
        //    Item item = 
        //}
    }
}
