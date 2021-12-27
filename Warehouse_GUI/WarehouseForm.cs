using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WarehouseGUI.Components;

namespace WarehouseGUI
{
    public partial class WarehouseForm : Form
    {
        public WarehouseForm()
        {
            Reference_Computer.CurrentForm = this;
            InitializeComponent();
            createRobotLabels();
            createPoints();
            createShelves();
            createDocks();
        }

        void createPoints()
        {
            Dictionary<char, Grid_Point[]> GridPointDict = new Dictionary<char, Grid_Point[]>();
            char[] charList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            int pointSideLength = Convert.ToInt32(this.Size.Height * 0.7 / WarehouseVars.numColumns);
            int row, col;
            List<Grid_Point> gp = new List<Grid_Point>();
            for(col = 0; col < WarehouseVars.numColumns; col++)
            {
                for(row = 0; row < WarehouseVars.numRows; row++)
                {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Name = String.Format("{0}{1}", charList[col], row + 1);
                    pictureBox.Size = new Size(pointSideLength, pointSideLength);
                    pictureBox.Image = refPt.Image;
                    pictureBox.Location = new Point(col * pointSideLength, row * pointSideLength);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox.SendToBack();
                    this.Controls.Add(pictureBox);

                    gp.Add(new Grid_Point(row, col, pictureBox));
                }
                Grid_Point[] gpArr = new Grid_Point[gp.Count];
                gp.CopyTo(gpArr);
                GridPointDict.Add(charList[col], (Grid_Point[])gpArr);
                gp.Clear();
            }
            Grid_Point.Grid_Point_Dict = GridPointDict;
        }

        void createRobotLabels()
        {
            for(int i = 0; i < WarehouseVars.numColumns; i++)
            {
                Components.RobotPosition.labelList.Add(
                    createRobot(i)
                );
            }
        }

        public Label createRobot(int id/*, Grid_Point startingPt*/)
        {
            RobotStatusTable.Rows.Add(new object[] { String.Format("Robot{0}", id), "A1", "Created" });
            RobotStatusTable.Size = new Size(RobotStatusTable.Size.Width, (id + 1) * 30);

            int pointSideLength = Convert.ToInt32(this.Size.Height * 0.7 / WarehouseVars.numColumns);
            Label label = new Label();
            label.Name = String.Format("Robot{0}", id);
            label.Text = label.Name;
            //label.Size = startingPt.pictureBox.Size;
            label.Size = new Size(pointSideLength, pointSideLength);
            //label.Location = startingPt.pictureBox.Location;
            label.Location = new Point(0, 0);
            label.Visible = true;
            label.Enabled = true;
            label.BackColor = Color.Red;
            label.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(label);
            label.BringToFront();
            return label;
        }

        void createShelves()
        {
            

            int xLength = Convert.ToInt32(this.Size.Height * 0.7 / WarehouseVars.numColumns) / 2;
            int yLength = Convert.ToInt32(this.Size.Height * 0.7 / WarehouseVars.numColumns) * (WarehouseVars.numRows - 2);
            for (int i = 0; i < WarehouseVars.numColumns - 1; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Name = String.Format("shelf{0}", i);
                pictureBox.Size = new Size(xLength, yLength);
                pictureBox.Image = refShelf.Image;
                int xPos = Grid_Point.GetGridPoint(i + 1, 0).pictureBox.Location.X - xLength / 2;
                int yPos = Grid_Point.GetGridPoint(0, 1).pictureBox.Location.Y;
                pictureBox.Location = new Point(xPos,yPos);
                //pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                this.Controls.Add(pictureBox);
                pictureBox.BringToFront();
            }
        }

        void createDocks()
        {
            int xLength = Grid_Point.GetGridPoint(0, 0).pictureBox.Size.Width;
            int yLength = Grid_Point.GetGridPoint(0, 0).pictureBox.Size.Height / 2;

            for(int i = 0; i < WarehouseVars.numDocks; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Name = String.Format("dock{0}", i);
                pictureBox.Size = new Size(xLength, yLength);
                pictureBox.Image = refDock.Image;
                int xPos = Grid_Point.GetGridPoint(i + 1, 0).pictureBox.Location.X;
                int yPos = Grid_Point.GetGridPoint(0, WarehouseVars.numRows - 1).pictureBox.Location.Y + yLength * 3 / 2;
                pictureBox.Location = new Point(xPos, yPos);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                this.Controls.Add(pictureBox);
                pictureBox.BringToFront();
            }
        }
        List<int> pastOrderIDs = new List<int>();
        public void updateOrderStatus(List<Order> orderList)
        {
            OrderTable.Rows.Clear();
            OrderTable.Size = new Size(OrderTable.Size.Width, (orderList.Count+1) * 25);
            foreach(Order order in orderList)
            {
                string message = "";
                foreach(string key in order.ItemDict.Keys)
                {
                    message += string.Format("{0}({1}), \n", key, order.ItemDict[key]);
                }
                if (!pastOrderIDs.Contains(order.orderID))
                {
                    OrderTable.Rows.Add(new object[] { message, order.Status });
                    pastOrderIDs.Add(order.orderID);
                }
                
            }
        }

        public void updateInventory(List<Item> itemList, int lowStockLimit)
        {
            Inventory.Rows.Clear();
            Inventory.Size = new Size(Inventory.Size.Width, itemList.Count * 25);
            foreach (Item item in itemList)
            {
                string message = "";
                if (item.quantity < lowStockLimit)
                {
                    message = "LOW STOCK";
                }
                Inventory.Rows.Add(new object[] { item.name, item.quantity, message});
            }
        }

        public void updateRobotStatus(int robotId, string location, string action)
        {
            RobotStatusTable.Rows[robotId].SetValues(new object[] { 
                String.Format("Robot{0}", robotId), 
                location, 
                action 
            });
        }

        public void createTruck(string id, int i)
        {
            TruckStatusTable.Rows.Add(new object[] { id, "Created" });
            TruckStatusTable.Size = new Size(TruckStatusTable.Size.Width, (i + 1) * 30);
        }

        public void updateTruckStatus(string truckId, string location, string action, int id)
        {
            TruckStatusTable.Rows[id].SetValues(new object[] {truckId, action});
        }
    }
}
