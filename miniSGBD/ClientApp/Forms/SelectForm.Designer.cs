namespace miniSGBD.Forms
{
    partial class SelectForm
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
            this.components = new System.ComponentModel.Container();
            this.label_table_config = new System.Windows.Forms.Label();
            this.button_table_config = new System.Windows.Forms.Button();
            this.label_column_config = new System.Windows.Forms.Label();
            this.button_column_config = new System.Windows.Forms.Button();
            this.list_column_config = new System.Windows.Forms.DataGridView();
            this.table_name = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.binding_source_table_name = new System.Windows.Forms.BindingSource(this.components);
            this.col_name = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.binding_source_column_name = new System.Windows.Forms.BindingSource(this.components);
            this.Aggregate = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.alias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.output = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.filter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group_by = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.having = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.list_join_config = new System.Windows.Forms.DataGridView();
            this.LeftTableName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LeftTableColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.RightTableName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.RightTableColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.list_column_config)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.binding_source_table_name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.binding_source_column_name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.list_join_config)).BeginInit();
            this.SuspendLayout();
            // 
            // label_table_config
            // 
            this.label_table_config.AutoSize = true;
            this.label_table_config.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_table_config.Location = new System.Drawing.Point(15, 19);
            this.label_table_config.Name = "label_table_config";
            this.label_table_config.Size = new System.Drawing.Size(124, 20);
            this.label_table_config.TabIndex = 0;
            this.label_table_config.Text = "Table Selection";
            // 
            // button_table_config
            // 
            this.button_table_config.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_table_config.Location = new System.Drawing.Point(1263, 42);
            this.button_table_config.Name = "button_table_config";
            this.button_table_config.Size = new System.Drawing.Size(50, 50);
            this.button_table_config.TabIndex = 2;
            this.button_table_config.Text = "+";
            this.button_table_config.UseVisualStyleBackColor = true;
            this.button_table_config.Click += new System.EventHandler(this.button_table_config_Click);
            // 
            // label_column_config
            // 
            this.label_column_config.AutoSize = true;
            this.label_column_config.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_column_config.Location = new System.Drawing.Point(15, 397);
            this.label_column_config.Name = "label_column_config";
            this.label_column_config.Size = new System.Drawing.Size(138, 20);
            this.label_column_config.TabIndex = 3;
            this.label_column_config.Text = "Selection Criteria";
            // 
            // button_column_config
            // 
            this.button_column_config.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_column_config.Location = new System.Drawing.Point(1124, 714);
            this.button_column_config.Name = "button_column_config";
            this.button_column_config.Size = new System.Drawing.Size(133, 30);
            this.button_column_config.TabIndex = 5;
            this.button_column_config.Text = "Select";
            this.button_column_config.UseVisualStyleBackColor = true;
            this.button_column_config.Click += new System.EventHandler(this.button_column_config_Click);
            // 
            // list_column_config
            // 
            this.list_column_config.BackgroundColor = System.Drawing.SystemColors.Window;
            this.list_column_config.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.list_column_config.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.table_name,
            this.col_name,
            this.Aggregate,
            this.alias,
            this.output,
            this.filter,
            this.group_by,
            this.having});
            this.list_column_config.GridColor = System.Drawing.SystemColors.GrayText;
            this.list_column_config.Location = new System.Drawing.Point(19, 420);
            this.list_column_config.Name = "list_column_config";
            this.list_column_config.RowHeadersWidth = 25;
            this.list_column_config.RowTemplate.Height = 24;
            this.list_column_config.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.list_column_config.Size = new System.Drawing.Size(1238, 288);
            this.list_column_config.TabIndex = 6;
            // 
            // table_name
            // 
            this.table_name.DataSource = this.binding_source_table_name;
            this.table_name.HeaderText = "Table Name";
            this.table_name.MinimumWidth = 6;
            this.table_name.Name = "table_name";
            this.table_name.Width = 125;
            // 
            // col_name
            // 
            this.col_name.DataSource = this.binding_source_column_name;
            this.col_name.HeaderText = "Column Name";
            this.col_name.MinimumWidth = 6;
            this.col_name.Name = "col_name";
            this.col_name.Width = 125;
            // 
            // Aggregate
            // 
            this.Aggregate.HeaderText = "Aggregate";
            this.Aggregate.Items.AddRange(new object[] {
            "COUNT",
            "MAX",
            "MIN",
            "SUM"});
            this.Aggregate.MinimumWidth = 6;
            this.Aggregate.Name = "Aggregate";
            this.Aggregate.Width = 125;
            // 
            // alias
            // 
            this.alias.HeaderText = "Alias";
            this.alias.MinimumWidth = 6;
            this.alias.Name = "alias";
            this.alias.Width = 125;
            // 
            // output
            // 
            this.output.HeaderText = "Output";
            this.output.MinimumWidth = 6;
            this.output.Name = "output";
            this.output.Width = 75;
            // 
            // filter
            // 
            this.filter.HeaderText = "Filter";
            this.filter.MinimumWidth = 6;
            this.filter.Name = "filter";
            this.filter.Width = 125;
            // 
            // group_by
            // 
            this.group_by.HeaderText = "Group By";
            this.group_by.MinimumWidth = 6;
            this.group_by.Name = "group_by";
            this.group_by.Width = 75;
            // 
            // having
            // 
            this.having.HeaderText = "Having";
            this.having.MinimumWidth = 6;
            this.having.Name = "having";
            this.having.Width = 125;
            // 
            // list_join_config
            // 
            this.list_join_config.BackgroundColor = System.Drawing.SystemColors.Window;
            this.list_join_config.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.list_join_config.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LeftTableName,
            this.LeftTableColumn,
            this.RightTableName,
            this.RightTableColumn});
            this.list_join_config.GridColor = System.Drawing.SystemColors.GrayText;
            this.list_join_config.Location = new System.Drawing.Point(19, 42);
            this.list_join_config.Name = "list_join_config";
            this.list_join_config.RowHeadersWidth = 25;
            this.list_join_config.RowTemplate.Height = 24;
            this.list_join_config.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.list_join_config.Size = new System.Drawing.Size(1238, 288);
            this.list_join_config.TabIndex = 7;
            // 
            // LeftTableName
            // 
            this.LeftTableName.DataSource = this.binding_source_table_name;
            this.LeftTableName.HeaderText = "Left Table Name";
            this.LeftTableName.MinimumWidth = 6;
            this.LeftTableName.Name = "LeftTableName";
            this.LeftTableName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LeftTableName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.LeftTableName.Width = 145;
            // 
            // LeftTableColumn
            // 
            this.LeftTableColumn.DataSource = this.binding_source_column_name;
            this.LeftTableColumn.HeaderText = "Left Table Column";
            this.LeftTableColumn.MinimumWidth = 6;
            this.LeftTableColumn.Name = "LeftTableColumn";
            this.LeftTableColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LeftTableColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.LeftTableColumn.Width = 155;
            // 
            // RightTableName
            // 
            this.RightTableName.DataSource = this.binding_source_table_name;
            this.RightTableName.HeaderText = "Right Table Name";
            this.RightTableName.MinimumWidth = 6;
            this.RightTableName.Name = "RightTableName";
            this.RightTableName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.RightTableName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.RightTableName.Width = 155;
            // 
            // RightTableColumn
            // 
            this.RightTableColumn.DataSource = this.binding_source_column_name;
            this.RightTableColumn.HeaderText = "Right Table Column";
            this.RightTableColumn.MinimumWidth = 6;
            this.RightTableColumn.Name = "RightTableColumn";
            this.RightTableColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.RightTableColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.RightTableColumn.Width = 160;
            // 
            // SelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1321, 756);
            this.Controls.Add(this.list_join_config);
            this.Controls.Add(this.list_column_config);
            this.Controls.Add(this.button_column_config);
            this.Controls.Add(this.label_column_config);
            this.Controls.Add(this.button_table_config);
            this.Controls.Add(this.label_table_config);
            this.Name = "SelectForm";
            this.Text = "Data Selection";
            ((System.ComponentModel.ISupportInitialize)(this.list_column_config)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.binding_source_table_name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.binding_source_column_name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.list_join_config)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_table_config;
        private System.Windows.Forms.Button button_table_config;
        private System.Windows.Forms.Label label_column_config;
        private System.Windows.Forms.Button button_column_config;
        private System.Windows.Forms.DataGridView list_column_config;
        private System.Windows.Forms.BindingSource binding_source_table_name;
        private System.Windows.Forms.BindingSource binding_source_column_name;
        private System.Windows.Forms.DataGridViewComboBoxColumn table_name;
        private System.Windows.Forms.DataGridViewComboBoxColumn col_name;
        private System.Windows.Forms.DataGridViewComboBoxColumn Aggregate;
        private System.Windows.Forms.DataGridViewTextBoxColumn alias;
        private System.Windows.Forms.DataGridViewCheckBoxColumn output;
        private System.Windows.Forms.DataGridViewTextBoxColumn filter;
        private System.Windows.Forms.DataGridViewCheckBoxColumn group_by;
        private System.Windows.Forms.DataGridViewTextBoxColumn having;
        private System.Windows.Forms.DataGridView list_join_config;
        private System.Windows.Forms.DataGridViewComboBoxColumn LeftTableName;
        private System.Windows.Forms.DataGridViewComboBoxColumn LeftTableColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn RightTableName;
        private System.Windows.Forms.DataGridViewComboBoxColumn RightTableColumn;
    }
}