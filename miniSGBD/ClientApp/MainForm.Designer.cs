namespace miniSGBD
{
    partial class MainForm
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
            this.databasesList = new System.Windows.Forms.ListView();
            this.tablesList = new System.Windows.Forms.ListView();
            this.addDB_btn = new System.Windows.Forms.Button();
            this.addTable_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.listView2 = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // databasesList
            // 
            this.databasesList.HideSelection = false;
            this.databasesList.Location = new System.Drawing.Point(12, 47);
            this.databasesList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.databasesList.Name = "databasesList";
            this.databasesList.Size = new System.Drawing.Size(143, 294);
            this.databasesList.TabIndex = 0;
            this.databasesList.UseCompatibleStateImageBehavior = false;
            this.databasesList.View = System.Windows.Forms.View.List;
            this.databasesList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.databasesList_MouseClick);
            // 
            // tablesList
            // 
            this.tablesList.HideSelection = false;
            this.tablesList.Location = new System.Drawing.Point(180, 47);
            this.tablesList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tablesList.Name = "tablesList";
            this.tablesList.Size = new System.Drawing.Size(144, 294);
            this.tablesList.TabIndex = 1;
            this.tablesList.UseCompatibleStateImageBehavior = false;
            this.tablesList.View = System.Windows.Forms.View.List;
            this.tablesList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tablesList_MouseClick);
            // 
            // addDB_btn
            // 
            this.addDB_btn.Location = new System.Drawing.Point(12, 345);
            this.addDB_btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.addDB_btn.Name = "addDB_btn";
            this.addDB_btn.Size = new System.Drawing.Size(143, 33);
            this.addDB_btn.TabIndex = 2;
            this.addDB_btn.Text = "Add";
            this.addDB_btn.UseVisualStyleBackColor = true;
            this.addDB_btn.Click += new System.EventHandler(this.addDB_buttonClick);
            // 
            // addTable_btn
            // 
            this.addTable_btn.Location = new System.Drawing.Point(180, 346);
            this.addTable_btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.addTable_btn.Name = "addTable_btn";
            this.addTable_btn.Size = new System.Drawing.Size(144, 32);
            this.addTable_btn.TabIndex = 3;
            this.addTable_btn.Text = "Add";
            this.addTable_btn.UseVisualStyleBackColor = true;
            this.addTable_btn.Click += new System.EventHandler(this.addTB_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Databases";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(176, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Tables";
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(352, 47);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(379, 142);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // listView2
            // 
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(352, 233);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(379, 142);
            this.listView2.TabIndex = 7;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.List;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(348, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Table Structure";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(348, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Table Contents";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 387);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addTable_btn);
            this.Controls.Add(this.addDB_btn);
            this.Controls.Add(this.tablesList);
            this.Controls.Add(this.databasesList);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "DBMS Main Menu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView databasesList;
        private System.Windows.Forms.ListView tablesList;
        private System.Windows.Forms.Button addDB_btn;
        private System.Windows.Forms.Button addTable_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}