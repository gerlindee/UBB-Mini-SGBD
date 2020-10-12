namespace miniSGBD
{
    partial class TestFormTables
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
            this.text_table_name = new System.Windows.Forms.TextBox();
            this.label_table_name = new System.Windows.Forms.Label();
            this.panel_table_column = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button_table_add_row = new System.Windows.Forms.Button();
            this.button_table_cancel = new System.Windows.Forms.Button();
            this.button_table_create = new System.Windows.Forms.Button();
            this.button_table_remove_row = new System.Windows.Forms.Button();
            this.button_add_reference = new System.Windows.Forms.Button();
            this.button_remove_reference = new System.Windows.Forms.Button();
            this.list_related_tables = new System.Windows.Forms.ListView();
            this.list_referenced_tables = new System.Windows.Forms.ListView();
            this.panel_table_column.SuspendLayout();
            this.SuspendLayout();
            // 
            // text_table_name
            // 
            this.text_table_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_table_name.Location = new System.Drawing.Point(17, 53);
            this.text_table_name.Name = "text_table_name";
            this.text_table_name.Size = new System.Drawing.Size(276, 30);
            this.text_table_name.TabIndex = 20;
            // 
            // label_table_name
            // 
            this.label_table_name.AutoSize = true;
            this.label_table_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_table_name.Location = new System.Drawing.Point(12, 21);
            this.label_table_name.Name = "label_table_name";
            this.label_table_name.Size = new System.Drawing.Size(148, 29);
            this.label_table_name.TabIndex = 19;
            this.label_table_name.Text = "Table name:";
            // 
            // panel_table_column
            // 
            this.panel_table_column.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel_table_column.ColumnCount = 6;
            this.panel_table_column.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panel_table_column.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panel_table_column.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panel_table_column.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panel_table_column.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panel_table_column.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panel_table_column.Controls.Add(this.label4, 3, 0);
            this.panel_table_column.Controls.Add(this.label3, 2, 0);
            this.panel_table_column.Controls.Add(this.label2, 1, 0);
            this.panel_table_column.Controls.Add(this.label1, 0, 0);
            this.panel_table_column.Controls.Add(this.label6, 4, 0);
            this.panel_table_column.Controls.Add(this.label7, 5, 0);
            this.panel_table_column.Location = new System.Drawing.Point(17, 104);
            this.panel_table_column.Name = "panel_table_column";
            this.panel_table_column.RowCount = 1;
            this.panel_table_column.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel_table_column.Size = new System.Drawing.Size(903, 377);
            this.panel_table_column.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(453, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 17);
            this.label4.TabIndex = 28;
            this.label4.Text = "Length";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(303, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 17);
            this.label3.TabIndex = 27;
            this.label3.Text = "Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 26;
            this.label2.Text = "Primary Key";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 25;
            this.label1.Text = "Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(603, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 17);
            this.label6.TabIndex = 30;
            this.label6.Text = "Unique";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(753, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 17);
            this.label7.TabIndex = 31;
            this.label7.Text = "Allow Null";
            // 
            // button_table_add_row
            // 
            this.button_table_add_row.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_table_add_row.Location = new System.Drawing.Point(927, 104);
            this.button_table_add_row.Margin = new System.Windows.Forms.Padding(4);
            this.button_table_add_row.Name = "button_table_add_row";
            this.button_table_add_row.Size = new System.Drawing.Size(51, 44);
            this.button_table_add_row.TabIndex = 22;
            this.button_table_add_row.Text = "+";
            this.button_table_add_row.UseVisualStyleBackColor = true;
            this.button_table_add_row.Click += new System.EventHandler(this.button_table_add_row_Click);
            // 
            // button_table_cancel
            // 
            this.button_table_cancel.Location = new System.Drawing.Point(820, 53);
            this.button_table_cancel.Margin = new System.Windows.Forms.Padding(4);
            this.button_table_cancel.Name = "button_table_cancel";
            this.button_table_cancel.Size = new System.Drawing.Size(100, 28);
            this.button_table_cancel.TabIndex = 24;
            this.button_table_cancel.Text = "Cancel";
            this.button_table_cancel.UseVisualStyleBackColor = true;
            this.button_table_cancel.Click += new System.EventHandler(this.button_table_cancel_Click);
            // 
            // button_table_create
            // 
            this.button_table_create.Location = new System.Drawing.Point(712, 53);
            this.button_table_create.Margin = new System.Windows.Forms.Padding(4);
            this.button_table_create.Name = "button_table_create";
            this.button_table_create.Size = new System.Drawing.Size(100, 28);
            this.button_table_create.TabIndex = 23;
            this.button_table_create.Text = "Create";
            this.button_table_create.UseVisualStyleBackColor = true;
            this.button_table_create.Click += new System.EventHandler(this.button_table_create_Click);
            // 
            // button_table_remove_row
            // 
            this.button_table_remove_row.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_table_remove_row.Location = new System.Drawing.Point(927, 156);
            this.button_table_remove_row.Margin = new System.Windows.Forms.Padding(4);
            this.button_table_remove_row.Name = "button_table_remove_row";
            this.button_table_remove_row.Size = new System.Drawing.Size(51, 44);
            this.button_table_remove_row.TabIndex = 25;
            this.button_table_remove_row.Text = "-";
            this.button_table_remove_row.UseVisualStyleBackColor = true;
            this.button_table_remove_row.Click += new System.EventHandler(this.button_table_remove_row_Click);
            // 
            // button_add_reference
            // 
            this.button_add_reference.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_add_reference.Location = new System.Drawing.Point(365, 558);
            this.button_add_reference.Margin = new System.Windows.Forms.Padding(4);
            this.button_add_reference.Name = "button_add_reference";
            this.button_add_reference.Size = new System.Drawing.Size(51, 44);
            this.button_add_reference.TabIndex = 28;
            this.button_add_reference.Text = ">";
            this.button_add_reference.UseVisualStyleBackColor = true;
            this.button_add_reference.Click += new System.EventHandler(this.button_add_reference_Click);
            // 
            // button_remove_reference
            // 
            this.button_remove_reference.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_remove_reference.Location = new System.Drawing.Point(365, 610);
            this.button_remove_reference.Margin = new System.Windows.Forms.Padding(4);
            this.button_remove_reference.Name = "button_remove_reference";
            this.button_remove_reference.Size = new System.Drawing.Size(51, 44);
            this.button_remove_reference.TabIndex = 29;
            this.button_remove_reference.Text = "<";
            this.button_remove_reference.UseVisualStyleBackColor = true;
            this.button_remove_reference.Click += new System.EventHandler(this.button_remove_reference_Click);
            // 
            // list_related_tables
            // 
            this.list_related_tables.FullRowSelect = true;
            this.list_related_tables.HideSelection = false;
            this.list_related_tables.Location = new System.Drawing.Point(19, 496);
            this.list_related_tables.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.list_related_tables.MultiSelect = false;
            this.list_related_tables.Name = "list_related_tables";
            this.list_related_tables.Size = new System.Drawing.Size(298, 193);
            this.list_related_tables.TabIndex = 30;
            this.list_related_tables.UseCompatibleStateImageBehavior = false;
            this.list_related_tables.View = System.Windows.Forms.View.List;
            // 
            // list_referenced_tables
            // 
            this.list_referenced_tables.HideSelection = false;
            this.list_referenced_tables.Location = new System.Drawing.Point(466, 497);
            this.list_referenced_tables.Name = "list_referenced_tables";
            this.list_referenced_tables.Size = new System.Drawing.Size(303, 192);
            this.list_referenced_tables.TabIndex = 31;
            this.list_referenced_tables.UseCompatibleStateImageBehavior = false;
            this.list_referenced_tables.View = System.Windows.Forms.View.List;
            // 
            // TestFormTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1014, 710);
            this.Controls.Add(this.list_referenced_tables);
            this.Controls.Add(this.list_related_tables);
            this.Controls.Add(this.button_remove_reference);
            this.Controls.Add(this.button_add_reference);
            this.Controls.Add(this.button_table_remove_row);
            this.Controls.Add(this.button_table_cancel);
            this.Controls.Add(this.button_table_create);
            this.Controls.Add(this.button_table_add_row);
            this.Controls.Add(this.panel_table_column);
            this.Controls.Add(this.text_table_name);
            this.Controls.Add(this.label_table_name);
            this.Name = "TestFormTables";
            this.Text = "TestFormTables";
            this.panel_table_column.ResumeLayout(false);
            this.panel_table_column.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox text_table_name;
        private System.Windows.Forms.Label label_table_name;
        private System.Windows.Forms.TableLayoutPanel panel_table_column;
        private System.Windows.Forms.Button button_table_add_row;
        private System.Windows.Forms.Button button_table_cancel;
        private System.Windows.Forms.Button button_table_create;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_table_remove_row;
        private System.Windows.Forms.Button button_add_reference;
        private System.Windows.Forms.Button button_remove_reference;
        private System.Windows.Forms.ListView list_related_tables;
        private System.Windows.Forms.ListView list_referenced_tables;
    }
}