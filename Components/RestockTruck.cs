using System;
namespace mongoTest.Components
{
    public class RestockTruck : Truck
    {
        public RestockTruck(Warehouse assignedWarehouse, int initPositionX, int initPositionY) :
            base(assignedWarehouse, initPositionX, initPositionY)
        { }
    }
}