namespace miniSGBD.Forms
{
    partial class SelectFormTables
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
            this.list_tables = new System.Windows.Forms.DataGridView();
            this.button_tables = new System.Windows.Forms.Button();
            this.label_tables = new System.Windows.Forms.Label();
            this.table_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.list_tables)).BeginInit();
            this.SuspendLayout();
            // 
            // list_tables
            // 
            this.list_tables.AllowUserToAddRows = false;
            this.list_tables.AllowUserToDeleteRows = false;
            this.list_tables.BackgroundColor = System.Drawing.SystemColors.Window;
            this.list_tables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.list_tables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.table_name,
            this.selected});
            this.list_tables.Location = new System.Drawing.Point(23, 69);
            this.list_tables.Name = "list_tables";
            this.list_tables.RowHeadersVisible = false;
            this.list_tables.RowHeadersWidth = 51;
            this.list_tables.RowTemplate.Height = 24;
            this.list_tables.Size = new System.Drawing.Size(272, 299);
            this.list_tables.TabIndex = 0;
            // 
            // button_tables
            // 
            this.button_tables.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_tables.Location = new System.Drawing.Point(203, 374);
            this.button_tables.Name = "button_tables";
            this.button_tables.Size = new System.Drawing.Size(92, 33);
            this.button_tables.TabIndex = 1;
            this.button_tables.Text = "Done";
            this.button_tables.UseVisualStyleBackColor = true;
            this.button_tables.Click += new System.EventHandler(this.button_tables_Click);
            // 
            // label_tables
            // 
            this.label_tables.AutoSize = true;
            this.label_tables.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_tables.Location = new System.Drawing.Point(19, 41);
            this.label_tables.Name = "label_tables";
            this.label_tables.Size = new System.Drawing.Size(188, 20);
            this.label_tables.TabIndex = 2;
            this.label_tables.Text = "Tables selected for join:";
            // 
            // table_name
            // 
            this.table_name.HeaderText = "Table Name";
            this.table_name.MinimumWidth = 6;
            this.table_name.Name = "table_name";
            this.table_name.ReadOnly = true;
            this.table_name.Width = 125;
            // 
            // selected
            // 
            this.selected.HeaderText = "Selected";
            this.selected.MinimumWidth = 6;
            this.selected.Name = "selected";
            this.selected.Width = 75;
            // 
            // StatementsFormTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(327, 430);
            this.Controls.Add(this.label_tables);
            this.Controls.Add(this.button_tables);
            this.Controls.Add(this.list_tables);
            this.Name = "StatementsFormTables";
            this.Text = "Tables For Data Selection";
            ((System.ComponentModel.ISupportInitialize)(this.list_tables)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView list_tables;
        private System.Windows.Forms.Button button_tables;
        private System.Windows.Forms.Label label_tables;
        private System.Windows.Forms.DataGridViewTextBoxColumn table_name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selected;
    }
}