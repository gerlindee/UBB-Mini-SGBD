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

        private List<TextBox> columnNames;
        private List<CheckBox> columnPrimaryKeys;
        private List<ComboBox> columnTypes;
        private List<TextBox> columnLengths;
        private List<CheckBox> columnUniques;
        private List<CheckBox> columnAllowNulls;

        private int rowCount = 0;
        private int rowIndex ;
        private int maxColumns = 9; // TODO: find how to get rid of maxrows 

        public TestFormTables(string _databaseName, Client _tcpClient)
        {
            databaseName = _databaseName;
            tcpClient = _tcpClient;
           
            InitializeItemArrays();
            InitializeComponent();
            InitializeRelatedTables();
            initiateEmptyFields();
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
            typesComboBox.Items.Add("FLOAT");  
            typesComboBox.Items.Add("DOUBLE");
            typesComboBox.Items.Add("DATE"); 
            typesComboBox.Items.Add("TIME");
            typesComboBox.Items.Add("DATETIME");
            typesComboBox.SelectedItem = "INT"; 

            return typesComboBox;
        }

        private void InitializeItemArrays()
        {
            columnNames = new List<TextBox>(maxColumns);
            columnPrimaryKeys = new List<CheckBox>(maxColumns);
            columnTypes = new List<ComboBox>(maxColumns);
            columnLengths = new List<TextBox>(maxColumns);
            columnUniques = new List<CheckBox>(maxColumns);
            columnAllowNulls = new List<CheckBox>(maxColumns);
        }

        private void InitializeRelatedTables()
        {
            tcpClient.Write(Commands.GET_ALL_TABLES + ';' + databaseName);
            var serverResponse = tcpClient.ReadFromServer();
            try
            {
                var tableNames = serverResponse.Split(';')[1].Split('|');
                foreach (string tableName in tableNames)
                {
                    if (tableName != "")
                    {
                        list_related_tables.Items.Add(tableName);
                    }
                }
            }
            catch (Exception) {}
        }

        private void button_table_add_row_Click(object sender, EventArgs e)
        { 
            if (rowCount == maxColumns)
            {
                MessageBox.Show("Maximum number of rows reached!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (validateColumn())
                {
                    rowCount++;
                    initiateEmptyFields();
                }
            }
        }

        private void initiateEmptyFields()
        {
            columnNames.Add(new TextBox());
            columnPrimaryKeys.Add(new CheckBox());
            columnTypes.Add(SetupColumnTypes());
            columnLengths.Add(new TextBox());
            columnUniques.Add(new CheckBox());
            columnAllowNulls.Add(new CheckBox());

            rowIndex = panel_table_column.RowCount++;
            panel_table_column.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            panel_table_column.Visible = false;

            panel_table_column.Controls.Add(columnNames[rowCount], 0, rowIndex);
            panel_table_column.Controls.Add(columnPrimaryKeys[rowCount], 1, rowIndex);
            panel_table_column.Controls.Add(columnTypes[rowCount], 2, rowIndex);
            panel_table_column.Controls.Add(columnLengths[rowCount], 3, rowIndex);
            panel_table_column.Controls.Add(columnUniques[rowCount], 4, rowIndex);
            panel_table_column.Controls.Add(columnAllowNulls[rowCount], 5, rowIndex);
            panel_table_column.Visible = true;
        }

        private void button_table_create_Click(object sender, EventArgs e)
        {
            if (text_table_name.Text.Equals(""))
            {
                MessageBox.Show(Validator.EMPTY_TABLE_NAME, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!checkAtLeastOnePK())
            {
                MessageBox.Show("At least one PK must be specified!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var message = Commands.CREATE_TABLE + ";" + databaseName + "#" + text_table_name.Text + "#";

            // Adding column values to the message
            for(int idx = 0; idx < rowCount; idx++)
            {
                message += columnNames[idx].Text + "|" + columnPrimaryKeys[idx].Checked.ToString() + "|"
                               + columnTypes[idx].SelectedItem.ToString() + "|" + columnLengths[idx].Text + "|"
                               + columnUniques[idx].Checked.ToString() + "|" + columnAllowNulls[idx].Checked.ToString() + "#";
            }

            // Adding FKs at the end of the message 
            foreach (ListViewItem referencedTables in list_referenced_tables.Items)
            {
                message += referencedTables.Text + "|";
            }

            tcpClient.Write(message);
            Close();
        }
        
        public bool validateColumn()
        {
            if (Validator.isStringEmpty(columnNames[rowCount].Text))
            {
                MessageBox.Show(Validator.EMPTY_COLUMN_NAME, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (Validator.isStringMaxLenght(columnNames[rowCount].Text))
            {
                MessageBox.Show(Validator.MAX_CHAR_TABLE_NAME, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            var selectedType = columnTypes[rowCount].SelectedItem.ToString();
            if (!columnLengths[rowCount].Text.Equals("") &&
                (selectedType.Equals("DATE") ||
                 selectedType.Equals("DATETIME") ||
                 selectedType.Equals("TIME")))
            {

                MessageBox.Show(Validator.INVALID_LENGHT_USE, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!Validator.isLengthCorrect(columnLengths[rowCount].Text) &&
                !selectedType.Equals("DATE") &&
                !selectedType.Equals("DATETIME") &&
                !selectedType.Equals("TIME"))
            {
                MessageBox.Show(Validator.LENGHT_NUMERICAL, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (Validator.isLengthMax(columnLengths[rowCount].Text))
            {
                MessageBox.Show(Validator.MAX_LENGTH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            return true;
        }

        public bool checkAtLeastOnePK()
        {
            return columnPrimaryKeys.Exists(x => x.Checked.ToString() == "True");
        }
        private void button_table_remove_row_Click(object sender, EventArgs e)
        {
            // TODO: daca mai e timp, sa se scoata ultima coloana adaugata
        }

        private void button_add_reference_Click(object sender, EventArgs e)
        {
            if (list_related_tables.Items.Count > 0)
            {
                var selectedTable = list_related_tables.SelectedItems[0];
                list_referenced_tables.Items.Add(selectedTable.Text);
                list_related_tables.Items.Remove(selectedTable);
            }
        }

        private void button_remove_reference_Click(object sender, EventArgs e)
        {
            if (list_referenced_tables.Items.Count > 0)
            {
                var selectedTable = list_referenced_tables.SelectedItems[0];
                list_related_tables.Items.Add(selectedTable.Text);
                list_referenced_tables.Items.Remove(selectedTable);
            }
        }

        private void button_table_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
