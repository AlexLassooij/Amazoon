﻿
namespace WarehouseGUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.col = new System.Windows.Forms.TextBox();
            this.robot = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.row = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.shelfheight = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dock = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(65, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of Cols:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(65, 238);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(350, 37);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of Initial Robots: ";
            // 
            // col
            // 
            this.col.Location = new System.Drawing.Point(443, 81);
            this.col.Name = "col";
            this.col.Size = new System.Drawing.Size(100, 23);
            this.col.TabIndex = 2;
            this.col.Text = "4";
            // 
            // robot
            // 
            this.robot.Location = new System.Drawing.Point(443, 252);
            this.robot.Name = "robot";
            this.robot.Size = new System.Drawing.Size(100, 23);
            this.robot.TabIndex = 3;
            this.robot.Text = "2";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.ForestGreen;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(387, 329);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(286, 77);
            this.button1.TabIndex = 4;
            this.button1.Text = "START SIMULATOR";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(65, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(231, 37);
            this.label3.TabIndex = 5;
            this.label3.Text = "Number of Rows";
            // 
            // row
            // 
            this.row.Location = new System.Drawing.Point(443, 23);
            this.row.Name = "row";
            this.row.Size = new System.Drawing.Size(100, 23);
            this.row.TabIndex = 6;
            this.row.Text = "4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(65, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(183, 37);
            this.label4.TabIndex = 7;
            this.label4.Text = "Shelf Height:";
            // 
            // shelfheight
            // 
            this.shelfheight.Location = new System.Drawing.Point(443, 139);
            this.shelfheight.Name = "shelfheight";
            this.shelfheight.Size = new System.Drawing.Size(100, 23);
            this.shelfheight.TabIndex = 8;
            this.shelfheight.Text = "3";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(65, 187);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(247, 37);
            this.label5.TabIndex = 9;
            this.label5.Text = "Number of Docks:";
            // 
            // dock
            // 
            this.dock.Location = new System.Drawing.Point(443, 201);
            this.dock.Name = "dock";
            this.dock.Size = new System.Drawing.Size(100, 23);
            this.dock.TabIndex = 10;
            this.dock.Text = "2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dock);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.shelfheight);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.row);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.robot);
            this.Controls.Add(this.col);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox col;
        private System.Windows.Forms.TextBox robot;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox row;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox shelfheight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox dock;
    }
}
