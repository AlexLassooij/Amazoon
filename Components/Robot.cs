using System;
using System.Threading;
using System.Collections.Generic;
using mongoTest.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace mongoTest.Components
{
    public class Robot
    {
        static public int MAX_WEIGHT = 1000;
        static public int MAX_CHARGE = 100;
        public static readonly int ENERGY_PER_TASK = 10; 
        
        public static readonly int ROBOT_CHARGING_TIME = 10000;
        private CentralComputer computer;
        private int batteryLevel;
        // weight of all items that the robot is current holding
        private int currentWeight = 0;
        private RobotTask currentTask;
        private int positionX = 0;
        private int positionY = 0;
        private List<Item> itemsInPossession = new List<Item>();
        private int robotId;
        private Mutex queueMutex;
        private Mutex truckMutex;
        IMongoCollection<Item> _items = ConnectionHelper.getItemCollection();



        public Robot(CentralComputer computer, Mutex queueMutex, Mutex truckMutex, int robotId)
        {
            this.batteryLevel = MAX_CHARGE;
            this.queueMutex = queueMutex;
            this.truckMutex = truckMutex;
            this.computer = computer;
            this.robotId = robotId;            
        }        

        // what does the robot need to do
        // check its battery level
        // consume tasks
        public void RunRobot() {
            while(true) {
                if (batterySufficientForTrip()) {
                    queueMutex.WaitOne();
                    bool successfulDequeue = computer.GetRobotTasks().TryDequeue(out currentTask);
                    queueMutex.ReleaseMutex();
                    if (successfulDequeue)
                    {
                        Console.WriteLine("Robot executing task");
                        ExecuteRobotTask(currentTask);
                    }
                } else {
                    ChargeBattery();
                }
                // Console.WriteLine("Robot running - " + robotId);
                Thread.Sleep(1000);
            }
        }   

        private void ExecuteRobotTask(RobotTask task)
        {
            if (task.getTaskType() == "load")
            {
                foreach (Item item in task.getItems())
                {                   
                    PickUpFromWarehouse(item);
                    currentWeight += item.weight;
                    itemsInPossession.Add(item);
                }
                LoadItemsIntoTruck(task.getAssignedTruck());

            } else if (task.getTaskType() == "unload") 
            {
                while (currentTask.assignedTruck.GetTruckState() != TruckState.Unloading)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Robot {robotId} waiting for truck {currentTask.assignedTruck.Id} to dock");
                }
                PickUpFromTruck();
                foreach (Item item in itemsInPossession)
                {                   
                    RestockItem(item);
                    Console.WriteLine($"Robot {robotId} has just restocked a {item.name} and is at" +
                        $" X: {positionX} and Y: {positionY}");
                    setItemStatus(item, ItemState.Available);
                }            
            }
            itemsInPossession.Clear();
            currentWeight = 0;
             // else if tasktype = restock
            // pickupitemfromtruck()
            // putintowarehouse, update item to available
            batteryLevel -= ENERGY_PER_TASK;
        }

        public void PickUpFromWarehouse(Item item)
        {
            foreach (ItemLocation location in computer.inventoryLocations)
            {
                if (location.items.Contains(item.Id))
                {
                    // moveToLocation is not fully implemented
                    if (location.orientation == "left") {
                        MoveToLocation(location.row, location.column);
                    } else {
                        MoveToLocation(location.row, location.column + 1);

                    }
                    break;
                }
            }
            // move robot to correct column first
            //moveToLocation(item.getLocation().row, item.getLocation().column);        
        }

        public void LoadItemsIntoTruck(Truck truck)
        {
            MoveToLocation(truck.GetDock().positionX, truck.GetDock().positionY);
            List<Item> itemsInTruck = currentTask.assignedTruck.GetItemsInTruck();
            foreach(Item item in itemsInPossession) {
                truckMutex.WaitOne();
                itemsInTruck.Add(item);
                truckMutex.ReleaseMutex();
                setItemStatus(item, ItemState.Loaded);
                Console.WriteLine($"Robot {robotId} loaded a {item.name} into truck. ItemId: {item.Id}");
            }
        }

        public void RestockItem(Item item) {
            foreach (ItemLocation location in computer.inventoryLocations)
            {
                if (location.items.Contains(item.Id))
                {
                    // moveToLocation is not fully implemented
                    if (location.orientation == "left") {
                        MoveToLocation(location.row, location.column);
                    } else {
                        MoveToLocation(location.row, location.column + 1);

                    }
                    break;
                }
            }
        }
        
        public void PickUpFromTruck() {
            MoveToLocation(currentTask.assignedTruck.GetDock().positionX, currentTask.assignedTruck.GetDock().positionY);
            //while (currentWeight <= MAX_WEIGHT && currentTask.assignedTruck.GetItemsInTruck().Count > 0) {
            //    truckMutex.WaitOne();
            //    List<Item> itemsInTruck = currentTask.assignedTruck.GetItemsInTruck();
            //    Item nextItem = itemsInTruck[itemsInTruck.Count - 1];  
            //    itemsInTruck.Remove(nextItem);   
            //    truckMutex.ReleaseMutex(); //multiple robots
            //    currentWeight += nextItem.weight;
            //    itemsInPossession.Add(nextItem);
            //    Console.WriteLine($"Robot {robotId} picked up a {nextItem.name} from truck. ItemId: {nextItem.Id}");
            //}

            foreach (Item item in currentTask.items)
            {
                truckMutex.WaitOne();
                List<Item> itemsInTruck = currentTask.assignedTruck.GetItemsInTruck();
                itemsInTruck.Remove(item);
                truckMutex.ReleaseMutex(); //multiple robots
                currentWeight += item.weight;
                itemsInPossession.Add(item);
                Console.WriteLine($"Robot {robotId} picked up a {item.name} from truck. ItemId: {item.Id}");
            }
        }

        public void setItemStatus(Item item, ItemState state) {
            item.itemState = state;
            FilterDefinition<Item> filter = new BsonDocument
                {
                    { "Id", item.Id },                    
                };

            UpdateDefinition<Item> update = Builders<Item>.Update.Set("itemState", state);
            _items.FindOneAndUpdate(filter, update);
        }
    
     

        // private int GetCurrentColumn() 
        // {
        //     return this.DesignatedAisle;

        // }

        private void ChargeBattery() {
            MoveToLocation(0, computer.GetWarehouse().getWarehouseRows());
            Thread.Sleep(ROBOT_CHARGING_TIME);
        }
            

        private void MoveToLocation(int row, int column) {
            if (column != GetRobotColumn())
            {
                // if the next item is in the top half of warehouse, move to top,
                // else move to bottom
                if (row < this.computer.GetWarehouse().getWarehouseRows() / 2)
                {
                    moveRobotVertically(0);
                } else
                {
                    moveRobotVertically(computer.GetWarehouse().getWarehouseRows() - 1);
                }     
                    moveRobotHorizontally(column);           
            }

            moveRobotVertically(row);
        }        

        private void moveRobotVertically(int row)
        {
            if (row > positionX)
            {
                while (row > positionX)
                {
                    positionX += 1;
                    Thread.Sleep(500);
                }
            } else
            {
                while (positionX > row)
                {
                    positionX -= 1;
                    Thread.Sleep(500);
                }
            }
        }

        private void moveRobotHorizontally(int column)
        {
            if (column > positionY)
            {
                while (column > positionY)
                {
                    positionY += 1;
                    Thread.Sleep(500);
                }
            }
            else
            {
                while (GetRobotColumn() > column)
                {
                    positionY -= 1;
                    Thread.Sleep(500);
                }
            }
        }

         private bool batterySufficientForTrip() {
            return batteryLevel >= 2 * ENERGY_PER_TASK;
        }

        static public int GetMaxWeight()
        {
            return MAX_WEIGHT;
        }
        private int GetRobotRow()
        {
            return this.positionX;
        }

        private int GetRobotColumn()
        {
            return this.positionY;
        }

        private int GetRobotId() {
            return this.robotId;
        }
    }
}
