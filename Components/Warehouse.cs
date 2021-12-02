using System;
using System.Threading;

namespace mongoTest.Components
{
    public class Warehouse
    {

        private CentralComputer centralComputer { get; set; }          
        
        // WAREHOUSE STUFF
        private int numRows;
        private int numColumns { get; set; }
        
        // DOCKING STATION STUFF
        private int numDocks { get; set; } // set the number of docking stations that will be used to initialize the docking stations in the warehouse
        private Dock[] docks; // array of docking stations to easily access each. dependent on the number of docking stations defined above
        
        // ROBOT STUFF
        private Robot[] robots;
        public int numRobots;

        // TRUCK stuff
        private Truck[] trucks;




        private int[] defaultRobotLocation = new int[2]{0, 0};

        public Warehouse(
            int numRows, 
            int numColumns,
            int numDocks,
            int numRobots)
        {            
            this.numRows = numRows;
            this.numColumns = numColumns;
            this.numDocks = numDocks;
            this.numRobots = numRobots;
            robots = new Robot[numRobots];
            docks = new Dock[numDocks];
            this.centralComputer = new CentralComputer(this);
        }

        public int getWarehouseRows()
        {
            return this.numRows;
        }

        public int getWarehouseColumns()
        {
            return this.numColumns;
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
    
        public Truck[] getTrucks()
        {
            return trucks;
        }
    }
}
