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
        private int[] location = new int[2] { 0, 0 };
        private List<Item> itemsInPossession;
        private String Id = Guid.NewGuid().ToString();
        private Mutex queueMutex;

        public Robot(CentralComputer computer, Mutex queueMutex)
        {
            this.batteryLevel = MAX_CHARGE;
            this.queueMutex = queueMutex;
            this.computer = computer;
            
        }        

        // what does the robot need to do
        // check its battery level
        // consume tasks
        public void runRobot() {
            while(true) {
                if (batterySufficientForTrip()) {
                    queueMutex.WaitOne();
                    bool successfulDequeue = computer.GetRobotTasks().TryDequeue(out currentTask);
                    queueMutex.ReleaseMutex();
                    if (successfulDequeue)
                    {
                        Console.WriteLine("Robot executing task");
                        executeRobotTask(currentTask);
                    }
                } else {
                    chargeBattery();
                }
                Console.WriteLine("Robot running - " + Id);
                Thread.Sleep(1000);
            }
        }   

        private void executeRobotTask(RobotTask task)
        {
            if (task.getTaskType() == "load")
            {
                foreach (Item item in task.getItems())
                {
                    pickUpItem(item);
                }
                loadTruck(task.getAssignedTruck());
            }
            batteryLevel -= ENERGY_PER_TASK;
        }

        private bool batterySufficientForTrip() {
            return batteryLevel >= 2 * ENERGY_PER_TASK;
        }

        static public int getMaxWeight()
        {
            return MAX_WEIGHT;
        }
        private int getRobotRow()
        {
            return this.location[0];
        }

        private int getRobotColumn()
        {
            return this.location[1];
        }

        private void chargeBattery() {
            moveToLocation(0, computer.getWarehouse().getWarehouseRows());
            Thread.Sleep(ROBOT_CHARGING_TIME);
        }

        public void pickUpItem(Item item)
        {
            // move robot to correct column first
            //moveToLocation(item.getLocation().row, item.getLocation().column);
            
        }

        private void moveToLocation(int row, int column) {
            if (column != getRobotColumn())
            {
                // if the next item is in the top half of warehouse, move to top,
                // else move to bottom
                if (row < this.computer.getWarehouse().getWarehouseRows() / 2)
                {
                    moveRobotVertically(0);
                } else
                {
                    moveRobotVertically(computer.getWarehouse().getWarehouseRows() - 1);
                }

                moveRobotHorizontally(column);
                moveRobotVertically(row);
            }
        }

        public void loadTruck(Truck truck)
        {

        }
     

        private void moveRobotVertically(int row)
        {
            if (row > getRobotRow())
            {
                while (row > getRobotRow())
                {
                    location[0] += 1;
                    Thread.Sleep(500);
                }
            } else
            {
                while (getRobotRow() > row)
                {
                    location[0] -= 1;
                    Thread.Sleep(500);
                }
            }
        }

        private void moveRobotHorizontally(int column)
        {
            if (column > getRobotColumn())
            {
                while (column > getRobotColumn())
                {
                    location[1] += 1;
                    Thread.Sleep(500);
                }
            }
            else
            {
                while (getRobotColumn() > column)
                {
                    location[1] -= 1;
                    Thread.Sleep(500);
                }
            }
        }
    }
}
