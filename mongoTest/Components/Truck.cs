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
        public List<Item> LoadedItems = new List<Item>();
        public int PositionX {get; set;}
        public int PositionY {get; set;}
        public Dock Dock {get; set;}
        public Semaphore TruckSemaphore;

        // Default constructor for adding truck to warehouse
        public Truck(Warehouse AssignedWarehouse, int PositionX, int PositionY, Semaphore TruckSemaphore)
        {
            this.AssignedWarehouse = AssignedWarehouse;
            this.PositionX = PositionX;
            this.PositionY = PositionY;
            this.TruckSemaphore = TruckSemaphore;
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
        
        public Dock FindAvailableDock()
        {
            TruckSemaphore.WaitOne();
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
                    // Console.WriteLine($"Dock {dock.DockID} occupied.");
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
            Dock = dockToReserve;
        }

        public bool IsDocked(Dock availableDock)
        {
            // if the truck position == available dock
            return (PositionX == availableDock.positionX && PositionY == availableDock.positionY);            
        }


        public abstract void NotifyArrival();

        // notifies computer that truck is docked and ready to be
        // loaded / unloaded
        public abstract void NotifyDocking();
        
        //shipping truck list and restock truck list
        public abstract void LeaveDock(); 
        
        private void NotifyDeparture(bool isTruckTaskDone)
        {
            // if some task for this truck is done
            // notify the central computer of its departure

            if (isTruckTaskDone)
            {
                this.TruckState = TruckState.Departed;
            }
        }        

        public List<Item> GetItemsInTruck() {
            return LoadedItems;
        }
        public TruckState GetTruckState()
        {
            return TruckState;
        }

        public int GetCurrentWeight()
        {
            int weight = 0;
            foreach (Item item in LoadedItems)
            {
                weight += item.weight;
            }

            return weight;
        }

        public int GetCurrentvolume()
        {
            int volume = 0;
            foreach (Item item in LoadedItems)
            {
                volume += item.volume;
            }

            return volume;
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

        public Dock GetDock()
        {
            return Dock;
        }
    }

}
