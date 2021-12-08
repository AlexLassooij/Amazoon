using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace WarehouseGUI.Components
{
    public static class Reference_Computer
    {
        public static List<RobotPosition> RobotPositionList = new List<RobotPosition>();
        public static WarehouseForm CurrentForm;

        public static void AddRobotToList(RobotPosition robot)
        {
            RobotPositionList.Add(robot);
        }

        public static void MoveRobotWithID(int id, int changeX, int changeY)
        {
            RobotPositionList[id-1].MoveRobot(changeX, changeY);
        }
    }

    public class RobotPosition { 
        public Grid_Point RobotGridPt;
        public Label label;
        public static List<Label> labelList = new List<Label>();

        public RobotPosition(Grid_Point pt, int id)
        {
            RobotGridPt = pt;
            label = labelList[id - 1];
        }

        public void MoveRobot(int changeX, int changeY)
        {
            int currentX = RobotGridPt.X_Pos;
            int currentY = RobotGridPt.Y_Pos;

            int newX = currentX + changeX;
            int newY = currentY + changeY;

            RobotGridPt = Grid_Point.GetGridPoint(newX, newY);
            label.Location = RobotGridPt.pictureBox.Location;
        }
    }

    public class Grid_Point
    {
        //The actual points
        public char X_Cood;
        public int Y_Cood;

        //The numeric points
        public int X_Pos, Y_Pos;

        //Picturebox control element
        public PictureBox pictureBox;

        public static Dictionary<char, Grid_Point[]> Grid_Point_Dict;

        public Grid_Point(char X_Cood, int Y_Cood, PictureBox pb)
        {
            this.pictureBox = pb;

            this.X_Cood = X_Cood;
            this.Y_Cood = Y_Cood;

            X_Pos = Array.IndexOf("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(), X_Cood);
            Y_Pos = Y_Cood - 1;
        }

        public Grid_Point(int X_Pos, int Y_Pos, PictureBox pb)
        {
            this.pictureBox = pb;

            this.X_Pos = X_Pos;
            this.Y_Pos = Y_Pos;

            X_Cood = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[X_Pos];
            Y_Cood = Y_Pos + 1;
        }

        public static Grid_Point GetGridPoint(char X, int Y)
        {
            return Grid_Point_Dict[X][Y - 1];
        }

        public static Grid_Point GetGridPoint(int X, int Y)
        {
            char key = Grid_Point_Dict.Keys.ToArray<char>()[X];
            return Grid_Point_Dict[key][Y];
        }
    }
}
