using System; 
using mongoTest.Models;
using System.Collections.Generic;

namespace mongoTest.Components

{
    public class RobotTask
    {
        public string taskType;
        public List<Item> items;
        public Truck assignedTruck;

        public RobotTask(string taskType, List<Item> items, Truck assignedTruck)
        {
            this.taskType = taskType;
            this.items = new List<Item>(items);
            this.assignedTruck = assignedTruck;
        }

        public string getTaskType()
        {
            return taskType;
        }

        public List<Item> getItems()
        {
            return items;
        }

        public Truck getAssignedTruck()
        {
            return assignedTruck;
        }

    }
}
