using System;
using System.Threading;
using mongoTest.Models;

namespace mongoTest.Components
{
    public class ShippingTruck : Truck
    {
        public ShippingTruck(Warehouse assignedWarehouse, int initPositionX, int initPositionY, Semaphore TruckSemaphore) :
            base(assignedWarehouse, initPositionX, initPositionY, TruckSemaphore)
        { }

        override
        public void RunTruck()
        {
            NotifyArrival();
            Dock truckDock;
            while ((truckDock = FindAvailableDock()) == null)
            {
                Console.WriteLine($"Truck {Id} waiting for available dock");
                Thread.Sleep(500);
            }

            MoveTruckToDockingStation(truckDock);            
            ReadyForLoading();            
        }

        override
        public void NotifyDocking()
        {
            // let the computer know that truck has arrived           
            TruckState = TruckState.Docked;
            Thread.Sleep(500);
        }

        override
        public void LeaveDock() {
            foreach (Item item in LoadedItems) {
                AssignedWarehouse.getComputer().inventory.Remove(item);
                foreach (ItemLocation location in AssignedWarehouse.getComputer().inventoryLocations) {
                    if (location.items.Contains(item.Id)) {
                        location.items.Remove(item.Id);
                    }
                }
                
            }
            AssignedWarehouse.GetShippingTrucks().Remove(this);
            TruckState = TruckState.Departed;
            Dock.setDockState(DockState.Available);
            TruckSemaphore.Release();
            Console.WriteLine($"Shipping truck {Id} has left the warehouse");
        }

        override
        public void NotifyArrival()
        {
            // let the computer know that truck has arrived           
            TruckState = TruckState.Arrived;
            Console.WriteLine($"Shipping truck {Id} has arrived and is currently at X: {PositionX} Y: {PositionY}");
        }

        public void ReadyForLoading()
        {
            Console.WriteLine($"Shipping truck is ready for loading.");
            TruckState = TruckState.Loading;

        }
    }    
}
