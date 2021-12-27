using System;
namespace mongoTest.Models
{
    public enum ItemState
    {
        //On truck
        Incoming,
        //In Shelf
        Available,
        Purchased,
        //Task queue to loaded
        Loading,
        //Dropped off at truck
        Loaded,
        //Truck leaves
        Shipped
    }
}


