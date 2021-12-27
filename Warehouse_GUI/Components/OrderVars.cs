using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseGUI.Components
{
    public struct Order
    {
        public Dictionary<string, int> ItemDict;
        public string Status;
        public static int capacity = 0;
        public int orderID;

        public Order(Dictionary<string,int> ItemDict, string Status)
        {
            this.ItemDict = ItemDict;
            this.Status = Status;
            capacity++;
            orderID = capacity;
        }
    }

    public struct Item
    {
        public string name;
        public int quantity;
        public Item(string name, int quantity)
        {
            this.name = name;
            this.quantity = quantity;
        }
    }
}
