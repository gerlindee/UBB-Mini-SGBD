namespace miniSGBD.Forms
{
    partial class StatementsForm
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
            this.binding_source_table_name = new System.Windows.Forms.BindingSource(this.components);
            this.binding_source_column_name = new System.Windows.Forms.BindingSource(this.components);
            this.binding_source_sorting = new System.Windows.Forms.BindingSource(this.components);
            this.panel_join_config = new System.Windows.Forms.FlowLayoutPanel();
            this.table_name = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.col_name = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.alias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.output = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.filter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group_by = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.having = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.list_column_config)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.binding_source_table_name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.binding_source_column_name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.binding_source_sorting)).BeginInit();
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
            this.button_table_config.Location = new System.Drawing.Point(1196, 42);
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
            this.button_column_config.Location = new System.Drawing.Point(1057, 714);
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
            this.list_column_config.Size = new System.Drawing.Size(1171, 288);
            this.list_column_config.TabIndex = 6;
            // 
            // panel_join_config
            // 
            this.panel_join_config.AutoSize = true;
            this.panel_join_config.BackColor = System.Drawing.SystemColors.Window;
            this.panel_join_config.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_join_config.Location = new System.Drawing.Point(19, 42);
            this.panel_join_config.Name = "panel_join_config";
            this.panel_join_config.Size = new System.Drawing.Size(1171, 337);
            this.panel_join_config.TabIndex = 7;
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
            // StatementsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1265, 756);
            this.Controls.Add(this.panel_join_config);
            this.Controls.Add(this.list_column_config);
            this.Controls.Add(this.button_column_config);
            this.Controls.Add(this.label_column_config);
            this.Controls.Add(this.button_table_config);
            this.Controls.Add(this.label_table_config);
            this.Name = "StatementsForm";
            this.Text = "Data Selection";
            ((System.ComponentModel.ISupportInitialize)(this.list_column_config)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.binding_source_table_name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.binding_source_column_name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.binding_source_sorting)).EndInit();
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
        private System.Windows.Forms.FlowLayoutPanel panel_join_config;
        private System.Windows.Forms.BindingSource binding_source_sorting;
        private System.Windows.Forms.DataGridViewComboBoxColumn table_name;
        private System.Windows.Forms.DataGridViewComboBoxColumn col_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn alias;
        private System.Windows.Forms.DataGridViewCheckBoxColumn output;
        private System.Windows.Forms.DataGridViewTextBoxColumn filter;
        private System.Windows.Forms.DataGridViewCheckBoxColumn group_by;
        private System.Windows.Forms.DataGridViewTextBoxColumn having;
    }
}