using System;


namespace mongoTest.Components
{
    public class DeliveryTruck : Truck
    {
        public DeliveryTruck(Warehouse assignedWarehouse, int initPositionX, int initPositionY) :
            base(assignedWarehouse, initPositionX, initPositionY)
        { }

        override
        public void RunTruck()
        {
            NotifyArrival();
        }
    }    
}
