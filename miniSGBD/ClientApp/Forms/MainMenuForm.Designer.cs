namespace miniSGBD
{
    partial class MainMenuForm
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
            this.table_structure_list = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.table_contents_list = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.table_contents_list)).BeginInit();
            this.SuspendLayout();
            // 
            // databasesList
            // 
            this.databasesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.databasesList.HideSelection = false;
            this.databasesList.Location = new System.Drawing.Point(12, 47);
            this.databasesList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.databasesList.Name = "databasesList";
            this.databasesList.Size = new System.Drawing.Size(143, 547);
            this.databasesList.TabIndex = 0;
            this.databasesList.UseCompatibleStateImageBehavior = false;
            this.databasesList.View = System.Windows.Forms.View.List;
            this.databasesList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.databasesList_MouseClick);
            // 
            // tablesList
            // 
            this.tablesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tablesList.HideSelection = false;
            this.tablesList.Location = new System.Drawing.Point(180, 47);
            this.tablesList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tablesList.Name = "tablesList";
            this.tablesList.Size = new System.Drawing.Size(144, 547);
            this.tablesList.TabIndex = 1;
            this.tablesList.UseCompatibleStateImageBehavior = false;
            this.tablesList.View = System.Windows.Forms.View.List;
            this.tablesList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tablesList_MouseClick);
            // 
            // addDB_btn
            // 
            this.addDB_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addDB_btn.Location = new System.Drawing.Point(12, 598);
            this.addDB_btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.addDB_btn.Name = "addDB_btn";
            this.addDB_btn.Size = new System.Drawing.Size(143, 33);
            this.addDB_btn.TabIndex = 2;
            this.addDB_btn.Text = "+";
            this.addDB_btn.UseVisualStyleBackColor = true;
            this.addDB_btn.Click += new System.EventHandler(this.addDB_buttonClick);
            // 
            // addTable_btn
            // 
            this.addTable_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addTable_btn.Location = new System.Drawing.Point(180, 598);
            this.addTable_btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.addTable_btn.Name = "addTable_btn";
            this.addTable_btn.Size = new System.Drawing.Size(144, 32);
            this.addTable_btn.TabIndex = 3;
            this.addTable_btn.Text = "+";
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
            // table_structure_list
            // 
            this.table_structure_list.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.table_structure_list.HideSelection = false;
            this.table_structure_list.Location = new System.Drawing.Point(352, 47);
            this.table_structure_list.Name = "table_structure_list";
            this.table_structure_list.Size = new System.Drawing.Size(686, 256);
            this.table_structure_list.TabIndex = 6;
            this.table_structure_list.UseCompatibleStateImageBehavior = false;
            this.table_structure_list.View = System.Windows.Forms.View.List;
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
            this.label4.Location = new System.Drawing.Point(348, 319);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Table Contents";
            // 
            // table_contents_list
            // 
            this.table_contents_list.AllowUserToAddRows = false;
            this.table_contents_list.AllowUserToDeleteRows = false;
            this.table_contents_list.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.table_contents_list.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.table_contents_list.Location = new System.Drawing.Point(352, 342);
            this.table_contents_list.Name = "table_contents_list";
            this.table_contents_list.ReadOnly = true;
            this.table_contents_list.RowHeadersVisible = false;
            this.table_contents_list.RowHeadersWidth = 51;
            this.table_contents_list.RowTemplate.Height = 24;
            this.table_contents_list.Size = new System.Drawing.Size(686, 289);
            this.table_contents_list.TabIndex = 10;
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1050, 642);
            this.Controls.Add(this.table_contents_list);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.table_structure_list);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addTable_btn);
            this.Controls.Add(this.addDB_btn);
            this.Controls.Add(this.tablesList);
            this.Controls.Add(this.databasesList);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainMenuForm";
            this.Text = "DBMS Main Menu";
            ((System.ComponentModel.ISupportInitialize)(this.table_contents_list)).EndInit();
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
        private System.Windows.Forms.ListView table_structure_list;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView table_contents_list;
    }
}