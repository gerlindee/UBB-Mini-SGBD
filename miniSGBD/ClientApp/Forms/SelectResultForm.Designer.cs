namespace miniSGBD.Forms
{
    partial class SelectResultForm
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
            this.table_contents_list = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.table_contents_list)).BeginInit();
            this.SuspendLayout();
            // 
            // table_contents_list
            // 
            this.table_contents_list.AllowUserToAddRows = false;
            this.table_contents_list.AllowUserToDeleteRows = false;
            this.table_contents_list.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.table_contents_list.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.table_contents_list.Location = new System.Drawing.Point(12, 51);
            this.table_contents_list.Name = "table_contents_list";
            this.table_contents_list.ReadOnly = true;
            this.table_contents_list.RowHeadersVisible = false;
            this.table_contents_list.RowHeadersWidth = 51;
            this.table_contents_list.RowTemplate.Height = 24;
            this.table_contents_list.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.table_contents_list.Size = new System.Drawing.Size(870, 312);
            this.table_contents_list.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Selection Results:";
            // 
            // StatementsResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(894, 377);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.table_contents_list);
            this.Name = "StatementsResultForm";
            this.Text = "StatementsResultForm";
            ((System.ComponentModel.ISupportInitialize)(this.table_contents_list)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView table_contents_list;
        private System.Windows.Forms.Label label1;
    }
}