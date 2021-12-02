using System;
namespace mongoTest.Components
{
    public class ItemLocation
    {
        public Warehouse currentWarehouse { get; set; }
        public int row { get; set; }
        public int column { get; set; }
        public int shelf { get; set; }
        public String orientation { get; set; }
        //public double current weight;

        public ItemLocation(Warehouse currentWarehouse, int row, int column, int shelf, String orientation)
        {
            this.currentWarehouse = currentWarehouse;
            this.row = row;
            this.column = column;
            this.shelf = shelf;
            this.orientation = orientation;
        }
    }
}
