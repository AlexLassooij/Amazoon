using System;
using System.Threading;
using System.Collections.Generic;

namespace mongoTest.Components
{
    public class Warehouse
    {

        private CentralComputer centralComputer { get; set; }          
        
        // WAREHOUSE STUFF
        private int numRows;
        private int numColumns; 
        private int shelfHeight;
        private int ID;
        public static int MAX_SHELF_WEIGHT = 1000;    
        // DOCKING STATION STUFF
        private int numDocks { get; set; } // set the number of docking stations that will be used to initialize the docking stations in the warehouse
        private Dock[] docks; // array of docking stations to easily access each. dependent on the number of docking stations defined above
        
        // ROBOT STUFF
        private Robot[] robots;
        public int numRobots;

        // TRUCK stuff
        private List<ShippingTruck> ShippingTrucks = new List<ShippingTruck>();
        private List<RestockTruck> RestockTrucks = new List<RestockTruck>();
        private int[] defaultRobotLocation = new int[2]{0, 0};

        public Warehouse(
            int ID,
            int numRows, 
            int numColumns,
            int shelfHeight,
            int numDocks,
            int numRobots)
        {
            this.ID = ID;
            this.numRows = numRows;
            this.numColumns = numColumns;
            this.shelfHeight = shelfHeight;
            this.numDocks = numDocks;
            this.numRobots = numRobots;
            robots = new Robot[numRobots];
            docks = new Dock[numDocks];
            centralComputer = new CentralComputer(this);
        }

        public int getWarehouseRows()
        {
            return this.numRows;
        }

        public int getWarehouseColumns()
        {
            return this.numColumns;
        }

        public int getShelfHeight()
        {
            return this.shelfHeight;
        }

        public int getNumDocks(){
            return numDocks;
        }
        public Dock[] getDocks()
        {
            return docks;
        }

        public Robot[] getRobots()
        {
            return robots;
        }
    
        public List<ShippingTruck> GetShippingTrucks()
        {
            return ShippingTrucks;
        }

        public void AddShippingTruck(ShippingTruck truck)
        {
            ShippingTrucks.Add(truck);
        }

        public List<RestockTruck> GetRestockTrucks()
        {
            return RestockTrucks;
        }

        public void AddRestockTruck(RestockTruck truck)
        {
            RestockTrucks.Add(truck);
        }

        public int getID()
        {
            return ID;
        }

        public void SetComputer(CentralComputer computer)
        {
            this.centralComputer = computer;
        }

        public CentralComputer getComputer()
        {
            return centralComputer;
        }
    }
}
