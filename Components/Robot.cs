using System;
using System.Threading;
using System.Collections.Generic;
using mongoTest.Models;

namespace mongoTest.Components
{
    public class Robot
    {
        static private int MAX_WEIGHT = 1000;
        static private int MAX_CHARGE = 100;
        public static readonly int ENERGY_PER_TASK = 10; 
        
        public static readonly int ROBOT_CHARGING_TIME = 10000;
        private CentralComputer computer;
        private int batteryLevel;
        private int currentWeight;
        private RobotTask currentTask;
        private int positionX = 0;
        private int positionY = 0;
        private List<Item> itemsInPossession;
        private String Id = Guid.NewGuid().ToString();
        private Mutex queueMutex;
        private int DesignatedAisle;

        public Robot(CentralComputer computer, Mutex queueMutex, int DesignatedAisle)
        {
            this.batteryLevel = MAX_CHARGE;
            this.queueMutex = queueMutex;
            this.computer = computer;
            this.DesignatedAisle = DesignatedAisle;
            
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
                Console.WriteLine("Robot running - " + Id);
                Thread.Sleep(1000);
            }
        }   

        private void ExecuteRobotTask(RobotTask task)
        {
            if (task.getTaskType() == "load")
            {
                foreach (Item item in task.getItems())
                {                   
                    PickUpItem(item);
                }
                LoadItemsIntoTruck(task.getAssignedTruck());
            }
            // else if tasktype = restock
            // pickupitemfromtruck()
            // putintowarehouse, update item to available
            batteryLevel -= ENERGY_PER_TASK;
        }

        // 

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

        private int GetCurrentColumn() 
        {
            return this.DesignatedAisle;

        }

        private void ChargeBattery() {
            MoveToLocation(0, computer.GetWarehouse().getWarehouseRows());
            Thread.Sleep(ROBOT_CHARGING_TIME);
        }
        
        public void PickUpItem(Item item)
        {
            foreach (ItemLocation location in computer.GetInventoryLocations())
            {
                if (location.items.Contains(item.Id))
                {
                    // moveToLocation is not fully implemented
                    MoveToLocation(location.row, location.column, location.orientation);
                    break;
                }
            }
            // move robot to correct column first
            //moveToLocation(item.getLocation().row, item.getLocation().column);
            
        }

        private void MoveToLocation(int row, int column, string orientation) {
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
                moveRobotVertically(row);
            }
        }

        // overloaded method for movign to either charging station or truck, because
        // orientation does not matter
        private void MoveToLocation(int row, int column)
        {
            if (column != GetRobotColumn())
            {
                // if the next item is in the top half of warehouse, move to top,
                // else move to bottom
                if (row < this.computer.GetWarehouse().getWarehouseRows() / 2)
                {
                    moveRobotVertically(0);
                }
                else
                {
                    moveRobotVertically(computer.GetWarehouse().getWarehouseRows() - 1);
                }

                moveRobotHorizontally(column);
                moveRobotVertically(row);
            }
        }

        public void LoadItemsIntoTruck(Truck truck)
        {

            MoveToLocation(truck.GetDock().positionX, truck.GetDock().positionY);
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
    }
}
