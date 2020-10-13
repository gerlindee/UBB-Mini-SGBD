namespace miniSGBD
{
    partial class AddDatabaseForm
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
            this.text_db_name = new System.Windows.Forms.TextBox();
            this.create_db_btn = new System.Windows.Forms.Button();
            this.cancel_db_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // text_db_name
            // 
            this.text_db_name.Font = new System.Drawing.Font("Mongolian Baiti", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_db_name.Location = new System.Drawing.Point(32, 73);
            this.text_db_name.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.text_db_name.Name = "text_db_name";
            this.text_db_name.Size = new System.Drawing.Size(259, 23);
            this.text_db_name.TabIndex = 0;
            // 
            // create_db_btn
            // 
            this.create_db_btn.Location = new System.Drawing.Point(32, 118);
            this.create_db_btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.create_db_btn.Name = "create_db_btn";
            this.create_db_btn.Size = new System.Drawing.Size(92, 25);
            this.create_db_btn.TabIndex = 1;
            this.create_db_btn.Text = "Create";
            this.create_db_btn.UseVisualStyleBackColor = true;
            this.create_db_btn.Click += new System.EventHandler(this.create_db_btn_Click);
            // 
            // cancel_db_btn
            // 
            this.cancel_db_btn.Location = new System.Drawing.Point(190, 118);
            this.cancel_db_btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cancel_db_btn.Name = "cancel_db_btn";
            this.cancel_db_btn.Size = new System.Drawing.Size(100, 25);
            this.cancel_db_btn.TabIndex = 2;
            this.cancel_db_btn.Text = "Cancel";
            this.cancel_db_btn.UseVisualStyleBackColor = true;
            this.cancel_db_btn.Click += new System.EventHandler(this.cancel_db_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Database name:";
            // 
            // AddDatabaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuBar;
            this.ClientSize = new System.Drawing.Size(336, 195);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel_db_btn);
            this.Controls.Add(this.create_db_btn);
            this.Controls.Add(this.text_db_name);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AddDatabaseForm";
            this.Text = "Create Database";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox text_db_name;
        private System.Windows.Forms.Button create_db_btn;
        private System.Windows.Forms.Button cancel_db_btn;
        private System.Windows.Forms.Label label1;
    }
}