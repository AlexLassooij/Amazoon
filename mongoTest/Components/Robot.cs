using System;
using System.Threading;
using System.Collections.Generic;
using mongoTest.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Windows.Forms;

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
        IMongoCollection<Order> _orders = ConnectionHelper.getOrderCollection();





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
                        Console.WriteLine($"Robot {robotId} executing task : {currentTask.taskType} truck {currentTask.assignedTruck.Id}");
                        WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
                        {
                            WarehouseGUI.Components.Reference_Computer.CurrentForm.updateRobotStatus(
                                robotId, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[positionX] + (positionY+1).ToString(),
                                $"Executing task : {currentTask.taskType} truck {currentTask.assignedTruck.Id}");
                        });
                        ExecuteRobotTask(currentTask);
                    }
                } else {
                    ChargeBattery();
                }
                // Console.WriteLine("Robot running - " + robotId);
                UpdateOrdersToShipped();
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
                    Console.WriteLine($"Robot {robotId} has picked up a {item.name}");
                    WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
                    {
                        WarehouseGUI.Components.Reference_Computer.CurrentForm.updateRobotStatus(
                            robotId, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[positionX] + (positionY + 1).ToString(),
                            $"Picked up a {item.name}");
                    });
                }
                LoadItemsIntoTruck(task.getAssignedTruck());

            } else if (task.getTaskType() == "unload")
            {
                while (currentTask.assignedTruck.GetTruckState() != TruckState.Unloading)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Robot {robotId} waiting for truck {currentTask.assignedTruck.Id} to dock");
                    WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
                    {
                        WarehouseGUI.Components.Reference_Computer.CurrentForm.updateRobotStatus(
                            robotId, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[positionX] + (positionY + 1).ToString(),
                            $"Waiting for truck {currentTask.assignedTruck.Id} to dock");
                    });
                }
                PickUpFromTruck();
                foreach (Item item in itemsInPossession)
                {
                    RestockItem(item);
                    Console.WriteLine($"Robot {robotId} has just restocked a {item.name} and is at" +
                        $" X: {positionX} and Y: {positionY}");
                    WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
                    {
                        WarehouseGUI.Components.Reference_Computer.CurrentForm.updateRobotStatus(
                            robotId, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[positionX] + (positionY + 1).ToString(),
                            $"Restocked a {item.name}");
                    });
                    SetItemStatus(item, ItemState.Available);
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
                        Console.WriteLine("breaks in 1");
                        MoveToLocation(location.row, location.column);
                    } else {
                        Console.WriteLine("breaks in 2");
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
            Console.WriteLine("breaks in 3");
            MoveToLocation(truck.GetDock().positionX, truck.GetDock().positionY);
            List<Item> itemsInTruck = currentTask.assignedTruck.GetItemsInTruck();
            foreach(Item item in itemsInPossession) {
                truckMutex.WaitOne();
                itemsInTruck.Add(item);
                truckMutex.ReleaseMutex();
                SetItemStatus(item, ItemState.Loaded);
                Console.WriteLine($"Robot {robotId} loaded a {item.name} into truck. ItemId: {item.Id}");
                WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
                {
                    WarehouseGUI.Components.Reference_Computer.CurrentForm.updateRobotStatus(
                        robotId, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[positionX] + (positionY + 1).ToString(),
                        $"Loaded a {item.name} into truck. ItemId: {item.Id}");
                });
            }
        }

        public void RestockItem(Item item) {
            foreach (ItemLocation location in computer.inventoryLocations)
            {
                if (location.items.Contains(item.Id))
                {
                    // moveToLocation is not fully implemented
                    if (location.orientation == "left") {
                        Console.WriteLine("breaks in 4");
                        MoveToLocation(location.row, location.column);
                    } else {
                        Console.WriteLine("breaks in 5");
                        MoveToLocation(location.row, location.column + 1);

                    }
                    break;
                }
            }
        }

        public void PickUpFromTruck() {
            Console.WriteLine("breaks in 6");
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
                WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
                {
                    WarehouseGUI.Components.Reference_Computer.CurrentForm.updateRobotStatus(
                        robotId, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[positionX] + (positionY + 1).ToString(),
                        $"Picked up a {item.name} from truck. ItemId: {item.Id}");
                });
            }
        }

        public void SetItemStatus(Item item, ItemState state)
        {
            List<Order> orders = _orders.Find(order => true).ToList();
            item.itemState = state;
            foreach (Order order in orders)
            {
                foreach (Item NextItem in order.items)
                {
                    if (NextItem.Id == item.Id)
                    {
                        _orders.DeleteOne(OrderToDelete => OrderToDelete.Id == order.Id);
                        order.items.Remove(NextItem);
                        order.items.Add(item);
                        _orders.InsertOne(order);
                        break;
                    }
                }

            }
            item.itemState = state;
            UpdateDefinition<Item> update = Builders<Item>.Update.Set("itemState", state);
            _items.UpdateOne(newItem => newItem.Id == item.Id, update);
        }

        public void UpdateOrdersToShipped()
        {
            List<Order> orders = _orders.Find(order => order.orderState == OrderState.Loading).ToList();
            UpdateDefinition<Order> update = Builders<Order>.Update.Set("orderState", OrderState.Shipped);

            foreach (Order order in orders)
            {
                bool itemsLoaded = true;
                foreach (Item item in order.items)
                {
                    if (item.itemState != ItemState.Loaded) itemsLoaded = false;
                }
                if (itemsLoaded)
                {
                    _orders.FindOneAndUpdate(orderToUpdate => orderToUpdate.Id == order.Id, update);
                }
            }
        }



        // private int GetCurrentColumn()
        // {
        //     return this.DesignatedAisle;

        // }

        private void ChargeBattery() {
            Console.WriteLine("breaks in 7");
            MoveToLocation(0, computer.GetWarehouse().getWarehouseRows());
            Thread.Sleep(ROBOT_CHARGING_TIME);
        }


        private void MoveToLocation(int row, int column)
        {
            if (column != GetRobotColumn())
            {
                // if the next item is in the top half of warehouse, move to top,
                // else move to bottom
                if (row < this.computer.GetWarehouse().getWarehouseRows() / 2)
                {
                    MoveRobotVertically(0);
                }
                else
                {
                    MoveRobotVertically(computer.GetWarehouse().getWarehouseRows() - 1);
                }
                MoveRobotHorizontally(column);
            }

            MoveRobotVertically(row);
        }

        private void MoveRobotHorizontally(int row)
        {
            if (row > positionX)
            {
                while (row > positionX)
                {
                    positionX += 1;
                    WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
                    {
                        WarehouseGUI.Components.Reference_Computer.MoveRobotWithID(robotId, positionX, positionY);
                    });
                    Thread.Sleep(500);
                }
            }
            else
            {
                while (positionX > row)
                {
                    positionX -= 1;
                    WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
                    {
                        WarehouseGUI.Components.Reference_Computer.MoveRobotWithID(robotId, positionX, positionY);
                    });
                    Thread.Sleep(500);
                }
            }

            Console.WriteLine($"Robot {robotId} is at X: {positionX} Y: {positionY}");
        }

        private void MoveRobotVertically(int column)
        {
            if (column > positionY)
            {
                while (column > positionY)
                {
                    positionY += 1;
                    WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
                    {
                        WarehouseGUI.Components.Reference_Computer.MoveRobotWithID(robotId, positionX, positionY);
                    });
                    Thread.Sleep(500);
                }
            }
            else
            {
                while (GetRobotColumn() > column)
                {
                    positionY -= 1;
                    WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
                    {
                        WarehouseGUI.Components.Reference_Computer.MoveRobotWithID(robotId, positionX, positionY);
                    });
                    Thread.Sleep(500);
                }
            }
            Console.WriteLine($"Robot {robotId} is at X: {positionX} Y: {positionY}");

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
