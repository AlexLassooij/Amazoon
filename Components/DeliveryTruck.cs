using System;
using System.Threading;

namespace mongoTest.Components
{
    public class ShippingTruck : Truck
    {
        public ShippingTruck(Warehouse assignedWarehouse, int initPositionX, int initPositionY) :
            base(assignedWarehouse, initPositionX, initPositionY)
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
            AssignedWarehouse.AddShippingTruck(this);
            Thread.Sleep(500);
        }

        public void ReadyForLoading()
        {
            Console.WriteLine($"Shipping truck is ready for loading.");
            TruckState = TruckState.Loading;

        }
    }    
}
