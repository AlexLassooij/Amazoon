using System;
using System.Collections.Generic;
using mongoTest.Models;
using System.Threading;

namespace mongoTest.Components
{
    public abstract class Truck
    {
        static public int MAX_WEIGHT = 5000;
        static public int MAX_VOLUME = 5000;
        public int currentWeight = 0;
        public int currentVolume = 0;
        public string Id = Guid.NewGuid().ToString();
        public DateTime TimeOfArrival { get; set; }
        public Warehouse AssignedWarehouse { get; set; }
        public TruckState TruckState { get; set; }
        public List<Item> LoadedItems { get; set; }
        public int PositionX {get; set;}
        public int PositionY {get; set;}
        private Dock Docks {get; set;}

        private int[] truckCapacity = new int[2]; // truckCapacity[0] = max carrying weight capacity
                                                  // truckCapacity[1] = max carrying volume capacity 
        private int[] truckCurrentCapacity = new int[2]; // truckCurrentCapacity[0] = current weight the truck is carrying
                                                         // truckCurrentCapacity[1] = current volume the truck is carrying

        // Default constructor for adding truck to warehouse
        public Truck(Warehouse AssignedWarehouse, int InitPositionX, int InitPositionY)
        {
            this.AssignedWarehouse = AssignedWarehouse;
            this.PositionX = InitPositionX;
            this.PositionY = InitPositionY;
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
        public abstract void RunTruck();


        public void ReadyToLeave(Dock truckDock)
        {
            Console.WriteLine($"Restock truck is empty and is leaving.");
            TruckState = TruckState.Departed;
            truckDock.setDockState(DockState.Available);
        }

        public Dock FindAvailableDock()
        {
            foreach(Dock dock in AssignedWarehouse.getDocks())
            {
                if (dock.isAvailable())
                {
                    // this ensures that no other truck attemps to use this dock while the truck makes its journey to the dock   
                    ReserveDock(dock); 
                    return dock;
                }
                else 
                {
                    Console.WriteLine($"Dock {dock.DockID} occupied.");
                }
            }
            return null;
        }

        public void MoveTruckToDockingStation(Dock availableDock)
        {
            // let's assume trucks move in grids
            // let trucks be somewhere outside of the warehouse to start
            // and it finds its way to the available docking station
            // by navigating row and columns

            MoveTruckHorizontally(availableDock.positionX);
            MoveTruckVertically(availableDock.positionY);

            if (IsDocked(availableDock))
            {
                availableDock.setDockState(DockState.Occupied);
                NotifyDocking();
                Console.WriteLine($"Truck {Id} has been docked at dock {availableDock.DockID}");
            }
            
            // returns the dockID of the docking station that the truck has docked
        }        

        private void MoveTruckVertically(int row)
        {
            if (row > PositionX)
            {
                while (row > PositionX)
                {
                    PositionX += 1;
                    Console.WriteLine($"Truck {Id} is now at position X: {PositionX} Y: {PositionY}");
                    Thread.Sleep(500);                    
                }
            }
            else
            {
                while (PositionX > row)
                {
                    PositionX -= 1;
                    Console.WriteLine($"Truck {Id} is now at position X: {PositionX} Y: {PositionY}");
                    Thread.Sleep(500);
                }
            }
        }

        private void MoveTruckHorizontally(int column)
        {
            if (column > PositionY)
            {
                while (column > PositionY)
                {
                    PositionY += 1;
                    Console.WriteLine($"Truck {Id} is now at position X: {PositionX} Y: {PositionY}");
                    Thread.Sleep(500);
                }
            }
            else
            {
                while (PositionY > column)
                {
                    PositionY -= 1;
                    Console.WriteLine($"Truck {Id} is now at position X: {PositionX} Y: {PositionY}");
                    Thread.Sleep(500);
                }
            }
        }

        public void ReserveDock(Dock dockToReserve)
        {
            dockToReserve.setDockState(DockState.Reserved);
        }

        public bool IsDocked(Dock availableDock)
        {
            // if the truck position == available dock
            return (PositionX == availableDock.positionX && PositionY == availableDock.positionY);            
        }
        

        public void NotifyArrival()
        {
            // let the computer know that truck has arrived           
            TruckState = TruckState.Arrived;
            Console.WriteLine($"Truck {Id} has arrived and is currently at X: {PositionX} Y: {PositionY}");

        }

        // notifies computer that truck is docked and ready to be
        // loaded / unloaded
        public abstract void NotifyDocking();
        

        private void NotifyDeparture(bool isTruckTaskDone)
        {
            // if some task for this truck is done
            // notify the central computer of its departure

            if (isTruckTaskDone)
            {
                this.TruckState = TruckState.Departed;
            }
        }        

        public TruckState GetTruckState()
        {
            return TruckState;
        }

        public int GetCurrentWeight()
        {
            return currentWeight;
        }

        public int GetCurrentvolume()
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
