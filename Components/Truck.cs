using System;
using System.Collections.Generic;
using mongoTest.Models;

namespace mongoTest.Components
{
    public class Truck
    {
        static private int MAX_WEIGHT = 5000;
        static private int MAX_VOLUME = 5000;
        private int currentWeight = 0;
        private int currentVolume = 0;
        private DateTime timeOfArrival { get; set; }
        private Warehouse assignedWarehouse { get; set; }
        private TruckState truckState { get; set; }
        private List<Item> loadedItems { get; set; }
        private int truckPositionX {get; set;}
        private int truckPositionY {get; set;}
        private Dock docks {get; set;}

        private int[] truckCapacity = new int[2]; // truckCapacity[0] = max carrying weight capacity
                                                  // truckCapacity[1] = max carrying volume capacity 
        private int[] truckCurrentCapacity = new int[2]; // truckCurrentCapacity[0] = current weight the truck is carrying
                                                         // truckCurrentCapacity[1] = current volume the truck is carrying

        // Default constructor for adding truck to warehouse
        public Truck(TruckState truckState, Warehouse assignedWarehouse)
        {
            this.truckState = truckState;            
            this.assignedWarehouse = assignedWarehouse;
            this.truckState = TruckState.Loading;
        }

        // overload constructor for arriving truck
        //public Truck(DateTime timeOfArrival, Warehouse assignedWarehouse)
        //{
        //    this.timeOfArrival = timeOfArrival;
        //    this.assignedWarehouse = assignedWarehouse;
        //    this.truckState = TruckState.Arriving;
        //}

        private Dock findAvailableDock(Dock[] listofDocks)
        {
            Dock availableDock;
            foreach(Dock dock in listofDocks)
            {
                if (dock.isAvailable())
                {
                    availableDock = dock;
                    return availableDock;
                }
                else 
                {
                    Console.WriteLine($"Dock {dock.DockID} occupied.");
                }
            }
            return null;
        }

        private int moveTruckToDockingStation(Truck truck, Dock availableDock)
        {
            // let's assume trucks move in grids
            // let trucks be somewhere outside of the warehouse to start
            // and it finds its way to the available docking station
            // by navigating row and columns

            int destinationX = availableDock.positionX;
            int destinationY = availableDock.positionY;

            int truckpositionX = truck.truckPositionX;
            int truckpositionY = truck.truckPositionY;

            // how far it is from the truck idling zone
            int differenceX = destinationX - truckPositionX;
            int differenceY = destinationY - truckpositionY;

            // update truck coordinate
            for (int i = 0; i < differenceX; i++)
            {
                truck.truckPositionX = truckpositionX - i;
                //wait for gui to update
                // once updated, continue the loop
            }

            for (int j = 0; j < differenceY; j++)
            {
                truck.truckPositionY = truckpositionY - j;
                //wait for gui to update
                // once updated, continue the loop
            }
            int dockID = isDocked(availableDock);
            
            // returns the dockID of the docking station that the truck has docked
            return dockID;
        }
        private int isDocked(Dock availableDock)
        {
            // if the truck position == available dock
            if (this.truckPositionX == availableDock.positionX && this.truckPositionY == availableDock.positionY)
            {
                return this.docks.DockID;
            }
            else return 69420;
        }
        private void notifyArrival(bool isTruckDocked)
        {
            // let the computer know that truck has arrived
            if (isTruckDocked)
            {
                this.truckState = TruckState.Arrived;
            }
        }
        private void notifyDeparture(bool isTruckTaskDone)
        {
            // if some task for this truck is done
            // notify the central computer of its departure

            if (isTruckTaskDone)
            {
                this.truckState = TruckState.Departed;
            }
        }        

        public TruckState getTruckState()
        {
            return truckState;
        }

        public int getCurrentWeight()
        {
            return currentWeight;
        }

        public int getCurrentvolume()
        {
            return currentVolume;
        }

        public int getAvailableWeight()
        {
            return MAX_WEIGHT - currentWeight;
        }

        public int getAvailableVolume()
        {
            return MAX_VOLUME - currentVolume;
        }
    }

}
