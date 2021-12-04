using System;
namespace mongoTest.Components
{
    public class RestockTruck : Truck
    {
        public RestockTruck(Warehouse assignedWarehouse, int initPositionX, int initPositionY) :
            base(assignedWarehouse, initPositionX, initPositionY)
        { }

        public void readyForUnloading()
        {
            Console.WriteLine($"Restock truck is carrying {base.getCurrentvolume()}kg worth of products, and is ready for unloading.");
            truckState = TruckState.Unloading;
            
        }
        public void readyToLeave()
        {
            Console.WriteLine($"Restock truck is empty and is leaving.");
            truckState = TruckState.Departed;
        }

        private void getRestockTruckLocation()
        {
            Console.WriteLine($"Restock truck at X: {positionX} Y: {positionY}");
        }
       
    }
}