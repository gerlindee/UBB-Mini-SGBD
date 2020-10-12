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
            this.SuspendLayout();
            // 
            // databasesList
            // 
            this.databasesList.HideSelection = false;
            this.databasesList.Location = new System.Drawing.Point(12, 71);
            this.databasesList.Name = "databasesList";
            this.databasesList.Size = new System.Drawing.Size(160, 367);
            this.databasesList.TabIndex = 0;
            this.databasesList.UseCompatibleStateImageBehavior = false;
            this.databasesList.View = System.Windows.Forms.View.List;
            this.databasesList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.databasesList_MouseClick);
            // 
            // tablesList
            // 
            this.tablesList.HideSelection = false;
            this.tablesList.Location = new System.Drawing.Point(199, 71);
            this.tablesList.Name = "tablesList";
            this.tablesList.Size = new System.Drawing.Size(161, 367);
            this.tablesList.TabIndex = 1;
            this.tablesList.UseCompatibleStateImageBehavior = false;
            this.tablesList.View = System.Windows.Forms.View.List;
            // 
            // addDB_btn
            // 
            this.addDB_btn.Location = new System.Drawing.Point(107, 24);
            this.addDB_btn.Name = "addDB_btn";
            this.addDB_btn.Size = new System.Drawing.Size(65, 41);
            this.addDB_btn.TabIndex = 2;
            this.addDB_btn.Text = "Add";
            this.addDB_btn.UseVisualStyleBackColor = true;
            this.addDB_btn.Click += new System.EventHandler(this.addDB_buttonClick);
            // 
            // addTable_btn
            // 
            this.addTable_btn.Location = new System.Drawing.Point(294, 24);
            this.addTable_btn.Name = "addTable_btn";
            this.addTable_btn.Size = new System.Drawing.Size(65, 40);
            this.addTable_btn.TabIndex = 3;
            this.addTable_btn.Text = "Add";
            this.addTable_btn.UseVisualStyleBackColor = true;
            this.addTable_btn.Click += new System.EventHandler(this.addTB_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.addTable_btn);
            this.Controls.Add(this.addDB_btn);
            this.Controls.Add(this.tablesList);
            this.Controls.Add(this.databasesList);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView databasesList;
        private System.Windows.Forms.ListView tablesList;
        private System.Windows.Forms.Button addDB_btn;
        private System.Windows.Forms.Button addTable_btn;
    }
}