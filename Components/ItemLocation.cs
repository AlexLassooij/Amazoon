using System;
using System.Collections.Generic;
namespace mongoTest.Components
{
    public class ItemLocation
    {
        
        public int row { get; set; }
        public int column { get; set; }
        public int shelf { get; set; }
        public string orientation { get; set; }
        public List<string> items = new List<string>();
        public int currentWeight = 0;

        public ItemLocation(int row, int column, int shelf, string orientation)
        {            
            this.row = row;
            this.column = column;
            this.shelf = shelf;
            this.orientation = orientation;
        }
    }
}
