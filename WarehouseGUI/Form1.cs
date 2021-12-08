using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarehouseGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                WarehouseVars.numRows = row.Text != "" ? int.Parse(row.Text) : 0;
                WarehouseVars.numColumns = col.Text != "" ? int.Parse(col.Text) : 0;
                WarehouseVars.shelfHeight = shelfheight.Text != "" ? int.Parse(shelfheight.Text) : 0;
                WarehouseVars.numDocks = dock.Text != "" ? int.Parse(dock.Text) : 0;
                WarehouseVars.numRobots = robot.Text != "" ? int.Parse(robot.Text) : 0;

                if(WarehouseVars.numRows <= 2)
                {
                    WarehouseVars.numRows = 3;
                    throw new Exception("Rows must be greater than 2");
                }
                if (WarehouseVars.numColumns <= 0)
                {
                    WarehouseVars.numColumns = 1;
                    throw new Exception("Columns must be greater than 0");
                }
                if (WarehouseVars.shelfHeight <= 0)
                {
                    WarehouseVars.shelfHeight = 1;
                    throw new Exception("Shelf height must be greater than 0");
                }
                if (WarehouseVars.numDocks <= 0)
                {
                    WarehouseVars.numDocks = 1;
                    throw new Exception("Number of docks must be greater than 0");
                }
                if (WarehouseVars.numRobots < 0) 
                {
                    WarehouseVars.numRobots = 0;
                    throw new Exception("Initial number of robots cannot be negative");
                }
                if(WarehouseVars.numRobots > WarehouseVars.numColumns)
                {
                    WarehouseVars.numRobots = WarehouseVars.numColumns;
                    throw new Exception("Cannot have more robots than columns");
                }

                this.Close();
            }
            catch (Exception ex){
                MessageBox.Show(ex.Message);
            }
            
            
        }
    }
}
