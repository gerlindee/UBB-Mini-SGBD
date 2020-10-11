using ClientApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace miniSGBD
{
    public partial class TestFormTables : Form
    {
        private string databaseName;
        private Client tcpClient;

        private TextBox[] columnNames;
        private CheckBox[] columnPrimaryKeys;
        private ComboBox[] columnTypes;
        private TextBox[] columnLengths;
        private CheckBox[] columnUniques;
        private CheckBox[] columnNotNulls;
        private ComboBox[] columnForeignKeys;

        private int rowCount = 0;
        private int rowIndex ;
        private int maxColumns = 9; // TODO: find how to get rid of maxrows 

        public TestFormTables(string _databaseName, Client _tcpClient)
        {
            databaseName = _databaseName;
            tcpClient = _tcpClient;
            InitializeItemArrays();
            InitializeComponent();
        }

        private ComboBox SetupColumnTypes()
        {
            var typesComboBox = new ComboBox();
            typesComboBox.Items.Add("CHAR");
            typesComboBox.Items.Add("VARCHAR");
            typesComboBox.Items.Add("TEXT");
            typesComboBox.Items.Add("MEDIUMTEXT");
            typesComboBox.Items.Add("LONGTEXT");
            typesComboBox.Items.Add("BOOLEAN");
            typesComboBox.Items.Add("INT");
            typesComboBox.Items.Add("FLOAT");  // doesn't need size, but needs precesion
            typesComboBox.Items.Add("DOUBLE"); // needs both size and precision
            typesComboBox.Items.Add("DATE"); 
            typesComboBox.Items.Add("TIME");
            typesComboBox.Items.Add("DATETIME");
            return typesComboBox;
        }

        private void InitializeItemArrays()
        {
            columnNames = new TextBox[maxColumns];
            columnPrimaryKeys = new CheckBox[maxColumns];
            columnTypes = new ComboBox[maxColumns];
            columnLengths = new TextBox[maxColumns];
            columnUniques = new CheckBox[maxColumns];
            columnNotNulls = new CheckBox[maxColumns];
            columnForeignKeys = new ComboBox[maxColumns];
    }

        private void SetupColumnForeignKeys(int row)
        {
            // TODO: once creating tables is a thing 
        }

        private void button_table_add_row_Click(object sender, EventArgs e)
        { 
            if (rowCount == maxColumns)
            {
                MessageBox.Show("Maximum number of rows reached!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                columnNames[rowCount] = new TextBox();
                columnPrimaryKeys[rowCount] = new CheckBox();
                columnTypes[rowCount] = SetupColumnTypes();
                columnLengths[rowCount] = new TextBox();
                columnUniques[rowCount] = new CheckBox();
                columnNotNulls[rowCount] = new CheckBox();
                columnForeignKeys[rowCount] = new ComboBox();

                rowIndex = panel_table_column.RowCount++;
                panel_table_column.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                panel_table_column.Visible = false;

                columnNames[rowCount].Dock = DockStyle.None;
                columnPrimaryKeys[rowCount].Dock = DockStyle.None;
                columnTypes[rowCount].Dock = DockStyle.None;
                columnLengths[rowCount].Dock = DockStyle.None;
                columnUniques[rowCount].Dock = DockStyle.None;
                columnNotNulls[rowCount].Dock = DockStyle.None;
                columnForeignKeys[rowCount].Dock = DockStyle.None;

                columnNames[rowCount].Anchor = AnchorStyles.None;
                columnPrimaryKeys[rowCount].Anchor = AnchorStyles.None;
                columnTypes[rowCount].Anchor = AnchorStyles.None;
                columnLengths[rowCount].Anchor = AnchorStyles.None;
                columnUniques[rowCount].Anchor = AnchorStyles.None;
                columnNotNulls[rowCount].Anchor = AnchorStyles.None;
                columnForeignKeys[rowCount].Anchor = AnchorStyles.None;

                panel_table_column.Controls.Add(columnNames[rowCount], 0, rowIndex);
                panel_table_column.Controls.Add(columnPrimaryKeys[rowCount], 1, rowIndex);
                panel_table_column.Controls.Add(columnTypes[rowCount], 2, rowIndex);
                panel_table_column.Controls.Add(columnLengths[rowCount], 3, rowIndex);
                panel_table_column.Controls.Add(columnUniques[rowCount], 4, rowIndex);
                panel_table_column.Controls.Add(columnNotNulls[rowCount], 5, rowIndex);
                panel_table_column.Controls.Add(columnForeignKeys[rowCount], 6, rowIndex);
                panel_table_column.Visible = true;

                rowCount++;
            }
        }

        private void button_table_create_Click(object sender, EventArgs e)
        {
            // TODO: validations
            var message = Commands.CREATE_TABLE + ";" + databaseName + "#" + text_table_name.Text + "#";

            for(int idx = 0; idx < rowCount; idx++)
            {
                message += columnNames[idx].Text + "|" + columnPrimaryKeys[idx].Checked.ToString() + "|"
                               + columnTypes[idx].SelectedItem.ToString() + "|" + columnLengths[idx].Text + "|"
                               + columnUniques[idx].Checked.ToString() + "|" + columnNotNulls[idx].Checked.ToString() + "|";

                if (columnForeignKeys[idx].SelectedItem == null)
                {
                    message += "Empty#";
                }
                else
                {
                    message += columnForeignKeys[idx].SelectedItem.ToString() + "#";
                }
            }
            tcpClient.Write(message); 
        }

        private void button_table_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
