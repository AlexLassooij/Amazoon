
namespace WarehouseGUI
{
    partial class WarehouseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.refPt = new System.Windows.Forms.PictureBox();
            this.refLabel = new System.Windows.Forms.Label();
            this.refShelf = new System.Windows.Forms.PictureBox();
            this.refDock = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.OrderTable = new System.Windows.Forms.DataGridView();
            this.ItemList = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.Inventory = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.RobotStatusTable = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.TruckStatusTable = new System.Windows.Forms.DataGridView();
            this.RobotName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TruckID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.refPt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refShelf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refDock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrderTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Inventory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RobotStatusTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TruckStatusTable)).BeginInit();
            this.SuspendLayout();
            // 
            // refPt
            // 
            this.refPt.Image = global::WarehouseGUI.Properties.Resources.space;
            this.refPt.Location = new System.Drawing.Point(12, 12);
            this.refPt.Name = "refPt";
            this.refPt.Size = new System.Drawing.Size(51, 49);
            this.refPt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.refPt.TabIndex = 0;
            this.refPt.TabStop = false;
            this.refPt.Visible = false;
            // 
            // refLabel
            // 
            this.refLabel.AutoSize = true;
            this.refLabel.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.refLabel.Location = new System.Drawing.Point(12, 151);
            this.refLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.refLabel.Name = "refLabel";
            this.refLabel.Size = new System.Drawing.Size(38, 15);
            this.refLabel.TabIndex = 1;
            this.refLabel.Text = "label1";
            this.refLabel.Visible = false;
            // 
            // refShelf
            // 
            this.refShelf.Image = global::WarehouseGUI.Properties.Resources.shelf;
            this.refShelf.Location = new System.Drawing.Point(12, 79);
            this.refShelf.Name = "refShelf";
            this.refShelf.Size = new System.Drawing.Size(51, 49);
            this.refShelf.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.refShelf.TabIndex = 2;
            this.refShelf.TabStop = false;
            this.refShelf.Visible = false;
            // 
            // refDock
            // 
            this.refDock.Image = global::WarehouseGUI.Properties.Resources.dock;
            this.refDock.Location = new System.Drawing.Point(12, 191);
            this.refDock.Name = "refDock";
            this.refDock.Size = new System.Drawing.Size(51, 49);
            this.refDock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.refDock.TabIndex = 3;
            this.refDock.TabStop = false;
            this.refDock.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Menu;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(525, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 32);
            this.label2.TabIndex = 5;
            this.label2.Text = "Active Orders";
            // 
            // OrderTable
            // 
            this.OrderTable.AllowUserToAddRows = false;
            this.OrderTable.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            this.OrderTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.OrderTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.OrderTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.OrderTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OrderTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemList,
            this.Status});
            this.OrderTable.Location = new System.Drawing.Point(525, 44);
            this.OrderTable.Name = "OrderTable";
            this.OrderTable.RowTemplate.Height = 25;
            this.OrderTable.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.OrderTable.Size = new System.Drawing.Size(565, 72);
            this.OrderTable.TabIndex = 6;
            // 
            // ItemList
            // 
            this.ItemList.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ItemList.HeaderText = "Items";
            this.ItemList.Name = "ItemList";
            this.ItemList.Width = 61;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.Width = 64;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Menu;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(525, 191);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 32);
            this.label1.TabIndex = 7;
            this.label1.Text = "Items";
            // 
            // Inventory
            // 
            this.Inventory.AllowUserToAddRows = false;
            this.Inventory.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            this.Inventory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.Inventory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.Inventory.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.Inventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Inventory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Message});
            this.Inventory.Location = new System.Drawing.Point(525, 234);
            this.Inventory.Name = "Inventory";
            this.Inventory.RowTemplate.Height = 25;
            this.Inventory.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.Inventory.Size = new System.Drawing.Size(565, 190);
            this.Inventory.TabIndex = 8;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.HeaderText = "Items";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 61;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn2.HeaderText = "Quantity";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 78;
            // 
            // Message
            // 
            this.Message.HeaderText = "Message";
            this.Message.Name = "Message";
            this.Message.Width = 78;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Menu;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(659, 533);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 32);
            this.label3.TabIndex = 9;
            this.label3.Text = "Robot Status";
            // 
            // RobotStatusTable
            // 
            this.RobotStatusTable.AllowUserToAddRows = false;
            this.RobotStatusTable.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Silver;
            this.RobotStatusTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.RobotStatusTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.RobotStatusTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.RobotStatusTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RobotStatusTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RobotName,
            this.Location,
            this.Action});
            this.RobotStatusTable.Location = new System.Drawing.Point(659, 568);
            this.RobotStatusTable.Name = "RobotStatusTable";
            this.RobotStatusTable.RowTemplate.Height = 25;
            this.RobotStatusTable.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.RobotStatusTable.Size = new System.Drawing.Size(558, 25);
            this.RobotStatusTable.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Menu;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(-2, 533);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 32);
            this.label4.TabIndex = 11;
            this.label4.Text = "Truck Status";
            // 
            // TruckStatusTable
            // 
            this.TruckStatusTable.AllowUserToAddRows = false;
            this.TruckStatusTable.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Silver;
            this.TruckStatusTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.TruckStatusTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.TruckStatusTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.TruckStatusTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TruckStatusTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TruckID,
            this.dataGridViewTextBoxColumn5});
            this.TruckStatusTable.Location = new System.Drawing.Point(-2, 568);
            this.TruckStatusTable.Name = "TruckStatusTable";
            this.TruckStatusTable.RowTemplate.Height = 25;
            this.TruckStatusTable.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TruckStatusTable.Size = new System.Drawing.Size(643, 25);
            this.TruckStatusTable.TabIndex = 12;
            // 
            // RobotName
            // 
            this.RobotName.HeaderText = "Robot Name";
            this.RobotName.Name = "RobotName";
            this.RobotName.Width = 99;
            // 
            // Location
            // 
            this.Location.HeaderText = "Location";
            this.Location.Name = "Location";
            this.Location.Width = 78;
            // 
            // Action
            // 
            this.Action.HeaderText = "Last Action";
            this.Action.Name = "Action";
            this.Action.Width = 91;
            // 
            // TruckID
            // 
            this.TruckID.HeaderText = "Truck ID";
            this.TruckID.Name = "TruckID";
            this.TruckID.Width = 74;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Last Action";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 91;
            // 
            // WarehouseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1266, 621);
            this.Controls.Add(this.TruckStatusTable);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.RobotStatusTable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Inventory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OrderTable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.refDock);
            this.Controls.Add(this.refShelf);
            this.Controls.Add(this.refLabel);
            this.Controls.Add(this.refPt);
            this.Name = "WarehouseForm";
            this.Text = "WarehouseForm";
            ((System.ComponentModel.ISupportInitialize)(this.refPt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refShelf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refDock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrderTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Inventory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RobotStatusTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TruckStatusTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox refPt;
        private System.Windows.Forms.Label refLabel;
        private System.Windows.Forms.PictureBox refShelf;
        private System.Windows.Forms.PictureBox refDock;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView OrderTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView Inventory;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView RobotStatusTable;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView TruckStatusTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn RobotName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Location;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn TruckID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    }
}