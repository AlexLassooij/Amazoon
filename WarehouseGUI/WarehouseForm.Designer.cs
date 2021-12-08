
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
            this.refPt = new System.Windows.Forms.PictureBox();
            this.refLabel = new System.Windows.Forms.Label();
            this.refShelf = new System.Windows.Forms.PictureBox();
            this.refDock = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.refPt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refShelf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refDock)).BeginInit();
            this.SuspendLayout();
            // 
            // refPt
            // 
            this.refPt.Image = global::WarehouseGUI.Properties.Resources.space;
            this.refPt.Location = new System.Drawing.Point(17, 20);
            this.refPt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.refPt.Name = "refPt";
            this.refPt.Size = new System.Drawing.Size(73, 82);
            this.refPt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.refPt.TabIndex = 0;
            this.refPt.TabStop = false;
            this.refPt.Visible = false;
            // 
            // refLabel
            // 
            this.refLabel.AutoSize = true;
            this.refLabel.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.refLabel.Location = new System.Drawing.Point(488, 165);
            this.refLabel.Name = "refLabel";
            this.refLabel.Size = new System.Drawing.Size(59, 25);
            this.refLabel.TabIndex = 1;
            this.refLabel.Text = "label1";
            this.refLabel.Visible = false;
            // 
            // refShelf
            // 
            this.refShelf.Image = global::WarehouseGUI.Properties.Resources.shelf;
            this.refShelf.Location = new System.Drawing.Point(255, 51);
            this.refShelf.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.refShelf.Name = "refShelf";
            this.refShelf.Size = new System.Drawing.Size(73, 82);
            this.refShelf.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.refShelf.TabIndex = 2;
            this.refShelf.TabStop = false;
            this.refShelf.Visible = false;
            // 
            // refDock
            // 
            this.refDock.Image = global::WarehouseGUI.Properties.Resources.dock;
            this.refDock.Location = new System.Drawing.Point(740, 165);
            this.refDock.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.refDock.Name = "refDock";
            this.refDock.Size = new System.Drawing.Size(73, 82);
            this.refDock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.refDock.TabIndex = 3;
            this.refDock.TabStop = false;
            this.refDock.Visible = false;
            // 
            // WarehouseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1367, 1035);
            this.Controls.Add(this.refDock);
            this.Controls.Add(this.refShelf);
            this.Controls.Add(this.refLabel);
            this.Controls.Add(this.refPt);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "WarehouseForm";
            this.Text = "WarehouseForm";
            ((System.ComponentModel.ISupportInitialize)(this.refPt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refShelf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refDock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox refPt;
        private System.Windows.Forms.Label refLabel;
        private System.Windows.Forms.PictureBox refShelf;
        private System.Windows.Forms.PictureBox refDock;
    }
}