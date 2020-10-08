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
            this.SuspendLayout();
            // 
            // label_db_name
            // 
            this.label_db_name.AutoSize = true;
            this.label_db_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_db_name.Location = new System.Drawing.Point(226, 148);
            this.label_db_name.Name = "label_db_name";
            this.label_db_name.Size = new System.Drawing.Size(187, 29);
            this.label_db_name.TabIndex = 0;
            this.label_db_name.Text = "Database name:";
            // 
            // text_db_name
            // 
            this.text_db_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_db_name.Location = new System.Drawing.Point(231, 180);
            this.text_db_name.Name = "text_db_name";
            this.text_db_name.Size = new System.Drawing.Size(276, 30);
            this.text_db_name.TabIndex = 1;
            // 
            // button_db_name
            // 
            this.button_db_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_db_name.Location = new System.Drawing.Point(403, 216);
            this.button_db_name.Name = "button_db_name";
            this.button_db_name.Size = new System.Drawing.Size(104, 31);
            this.button_db_name.TabIndex = 2;
            this.button_db_name.Text = "Create";
            this.button_db_name.UseVisualStyleBackColor = true;
            this.button_db_name.Click += new System.EventHandler(this.button_db_name_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_db_name);
            this.Controls.Add(this.text_db_name);
            this.Controls.Add(this.label_db_name);
            this.Name = "TestForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_db_name;
        private System.Windows.Forms.TextBox text_db_name;
        private System.Windows.Forms.Button button_db_name;
    }
}

