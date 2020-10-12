namespace ClientApp
{
    partial class TestForm
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
            this.label_db_name = new System.Windows.Forms.Label();
            this.text_db_name = new System.Windows.Forms.TextBox();
            this.button_db_name = new System.Windows.Forms.Button();
            this.button_db_delete = new System.Windows.Forms.Button();
            this.button_db_show_all = new System.Windows.Forms.Button();
            this.button_db_show_all_tables = new System.Windows.Forms.Button();
            this.button_db_create_table_nav = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_db_name
            // 
            this.label_db_name.AutoSize = true;
            this.label_db_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_db_name.Location = new System.Drawing.Point(254, 185);
            this.label_db_name.Name = "label_db_name";
            this.label_db_name.Size = new System.Drawing.Size(223, 32);
            this.label_db_name.TabIndex = 0;
            this.label_db_name.Text = "Database name:";
            // 
            // text_db_name
            // 
            this.text_db_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_db_name.Location = new System.Drawing.Point(260, 225);
            this.text_db_name.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.text_db_name.Name = "text_db_name";
            this.text_db_name.Size = new System.Drawing.Size(310, 35);
            this.text_db_name.TabIndex = 1;
            this.text_db_name.TextChanged += new System.EventHandler(this.text_db_name_TextChanged);
            // 
            // button_db_name
            // 
            this.button_db_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_db_name.Location = new System.Drawing.Point(260, 270);
            this.button_db_name.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_db_name.Name = "button_db_name";
            this.button_db_name.Size = new System.Drawing.Size(150, 39);
            this.button_db_name.TabIndex = 2;
            this.button_db_name.Text = "Create";
            this.button_db_name.UseVisualStyleBackColor = true;
            this.button_db_name.Click += new System.EventHandler(this.button_db_name_Click);
            // 
            // button_db_delete
            // 
            this.button_db_delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_db_delete.Location = new System.Drawing.Point(416, 270);
            this.button_db_delete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_db_delete.Name = "button_db_delete";
            this.button_db_delete.Size = new System.Drawing.Size(154, 39);
            this.button_db_delete.TabIndex = 4;
            this.button_db_delete.Text = "Delete";
            this.button_db_delete.UseVisualStyleBackColor = true;
            this.button_db_delete.Click += new System.EventHandler(this.button_db_delete_Click);
            // 
            // button_db_show_all
            // 
            this.button_db_show_all.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_db_show_all.Location = new System.Drawing.Point(260, 316);
            this.button_db_show_all.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_db_show_all.Name = "button_db_show_all";
            this.button_db_show_all.Size = new System.Drawing.Size(310, 39);
            this.button_db_show_all.TabIndex = 5;
            this.button_db_show_all.Text = "Display all Databases";
            this.button_db_show_all.UseVisualStyleBackColor = true;
            this.button_db_show_all.Click += new System.EventHandler(this.button_db_show_all_Click);
            // 
            // button_db_show_all_tables
            // 
            this.button_db_show_all_tables.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_db_show_all_tables.Location = new System.Drawing.Point(260, 362);
            this.button_db_show_all_tables.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_db_show_all_tables.Name = "button_db_show_all_tables";
            this.button_db_show_all_tables.Size = new System.Drawing.Size(310, 39);
            this.button_db_show_all_tables.TabIndex = 6;
            this.button_db_show_all_tables.Text = "Display all Tables";
            this.button_db_show_all_tables.UseVisualStyleBackColor = true;
            this.button_db_show_all_tables.Click += new System.EventHandler(this.button_db_show_all_tables_Click);
            // 
            // button_db_create_table_nav
            // 
            this.button_db_create_table_nav.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_db_create_table_nav.Location = new System.Drawing.Point(260, 409);
            this.button_db_create_table_nav.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_db_create_table_nav.Name = "button_db_create_table_nav";
            this.button_db_create_table_nav.Size = new System.Drawing.Size(310, 39);
            this.button_db_create_table_nav.TabIndex = 7;
            this.button_db_create_table_nav.Text = "Create a Table";
            this.button_db_create_table_nav.UseVisualStyleBackColor = true;
            this.button_db_create_table_nav.Click += new System.EventHandler(this.button_db_create_table_nav_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(900, 562);
            this.Controls.Add(this.button_db_create_table_nav);
            this.Controls.Add(this.button_db_show_all_tables);
            this.Controls.Add(this.button_db_show_all);
            this.Controls.Add(this.button_db_delete);
            this.Controls.Add(this.button_db_name);
            this.Controls.Add(this.text_db_name);
            this.Controls.Add(this.label_db_name);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TestForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_db_name;
        private System.Windows.Forms.TextBox text_db_name;
        private System.Windows.Forms.Button button_db_name;
        private System.Windows.Forms.Button button_db_delete;
        private System.Windows.Forms.Button button_db_show_all;
        private System.Windows.Forms.Button button_db_show_all_tables;
        private System.Windows.Forms.Button button_db_create_table_nav;
    }
}

