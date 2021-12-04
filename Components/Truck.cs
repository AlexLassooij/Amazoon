using System;
using System.Collections.Generic;
using mongoTest.Models;
using System.Threading;

namespace mongoTest.Components
{
    public class Truck
    {
        static private int MAX_WEIGHT = 5000;
        static private int MAX_VOLUME = 5000;
        private int currentWeight = 0;
        private int currentVolume = 0;
        private string Id = Guid.NewGuid().ToString();
        private DateTime timeOfArrival { get; set; }
        private Warehouse assignedWarehouse { get; set; }
        public TruckState truckState { get; set; }
        private List<Item> loadedItems { get; set; }
        public int positionX {get; set;}
        public int positionY {get; set;}
        private Dock docks {get; set;}

        private int[] truckCapacity = new int[2]; // truckCapacity[0] = max carrying weight capacity
                                                  // truckCapacity[1] = max carrying volume capacity 
        private int[] truckCurrentCapacity = new int[2]; // truckCurrentCapacity[0] = current weight the truck is carrying
                                                         // truckCurrentCapacity[1] = current volume the truck is carrying

        // Default constructor for adding truck to warehouse
        public Truck(Warehouse assignedWarehouse, int initPositionX, int initPositionY)
        {
            this.assignedWarehouse = assignedWarehouse;
            this.positionX = initPositionX;
            this.positionY = initPositionY;
        }

        // overload constructor for arriving truck
        //public Truck(DateTime timeOfArrival, Warehouse assignedWarehouse)
        //{
        //    this.timeOfArrival = timeOfArrival;
        //    this.assignedWarehouse = assignedWarehouse;
        //    this.truckState = TruckState.Arriving;
        //}

        // truck says it has arrived
        // truck searches through docks of the warehouse to see if any are available
        // if not, it will wait in a loop until one becomes available
        // once available, the truck will move towards the dock and dock itself there 
        public bool runTruck()
        {            
            notifyArrival();
            Dock truckDock = null;
            while ((truckDock = findAvailableDock()) == null)
            {
                Console.WriteLine($"Truck {Id} waiting for available dock");
                Thread.Sleep(500);
            }
            // this ensures that no other truck attemps to use this dock while the truck makes its journey to the dock
            reserveDock(truckDock);
            moveTruckToDockingStation(truckDock);
            
            return isDocked(truckDock);
            
        }

        private Dock findAvailableDock()
        {
            foreach(Dock dock in assignedWarehouse.getDocks())
            {
                if (dock.isAvailable())
                {                    
                    return dock;
                }
                else 
                {
                    Console.WriteLine($"Dock {dock.DockID} occupied.");
                }
            }
            return null;
        }

        private void moveTruckToDockingStation(Dock availableDock)
        {
            // let's assume trucks move in grids
            // let trucks be somewhere outside of the warehouse to start
            // and it finds its way to the available docking station
            // by navigating row and columns

            moveTruckHorizontally(availableDock.positionX);
            moveTruckVertically(availableDock.positionY);

            if (isDocked(availableDock))
            {
                availableDock.setDockState(DockState.Occupied);
                notifyDocking();
                Console.WriteLine($"Truck {Id} has been docked at dock {availableDock.DockID}");
            }
            
            // returns the dockID of the docking station that the truck has docked
        }        

        private void moveTruckVertically(int row)
        {
            if (row > positionX)
            {
                while (row > positionX)
                {
                    positionX += 1;
                    Console.WriteLine($"Truck {Id} is now at position X: {positionX} Y: {positionY}");
                    Thread.Sleep(500);                    
                }
            }
            else
            {
                while (positionX > row)
                {
                    positionX -= 1;
                    Console.WriteLine($"Truck {Id} is now at position X: {positionX} Y: {positionY}");
                    Thread.Sleep(500);
                }
            }
        }

        private void moveTruckHorizontally(int column)
        {
            if (column > positionY)
            {
                while (column > positionY)
                {
                    positionY += 1;
                    Console.WriteLine($"Truck {Id} is now at position X: {positionX} Y: {positionY}");
                    Thread.Sleep(500);
                }
            }
            else
            {
                while (positionY > column)
                {
                    positionY -= 1;
                    Console.WriteLine($"Truck {Id} is now at position X: {positionX} Y: {positionY}");
                    Thread.Sleep(500);
                }
            }
        }

        private void reserveDock(Dock dockToReserve)
        {
            dockToReserve.setDockState(DockState.Reserved);
        }

        private bool isDocked(Dock availableDock)
        {
            // if the truck position == available dock
            return (positionX == availableDock.positionX && positionY == availableDock.positionY);            
        }
        

        private void notifyArrival()
        {
            // let the computer know that truck has arrived           
            truckState = TruckState.Arrived;
            Console.WriteLine($"Truck {Id} has arrived and is currently at X: {positionX} Y: {positionY}");

        }

        // notifies computer that truck is docked and ready to be
        // loaded / unloaded
        private void notifyDocking()
        {
            // let the computer know that truck has arrived           
            truckState = TruckState.Docked;
            assignedWarehouse.addTruck(this);
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

        public void addWeight(int weight)
        {
            currentWeight += weight;
        }

        public void addVolume(int volume)
        {
            currentVolume += volume;
        }
    }

}
