using System;
namespace mongoTest.Models
{
    // will need to think about when to make state changes,
    // remember items of an order could be distributed across
    // multiple warehouses and trucks,
    // --> aim to spread orders across the least amount of warehouses and trucks
    // state changes when every single item of the order reaches the same state
    public enum OrderState
    {
        Placed,
        Loading,
        Shipped,
        Delivered
    }
}
