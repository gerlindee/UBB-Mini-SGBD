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

namespace miniSGBD.Forms
{
    public partial class StatementsForm : Form
    {
        private string databaseName;
        private Client tcpClient;
        private List<string> selectedTables;
        private List<KeyValuePair<string, List<string>>> tableColumns;  

        public StatementsForm(string _databaseName, Client _tcpClient)
        {
            databaseName = _databaseName;
            tcpClient = _tcpClient;
            selectedTables = new List<string>();
            tableColumns = new List<KeyValuePair<string, List<string>>>();
            InitializeComponent();
            list_column_config.CellValueChanged += new DataGridViewCellEventHandler(list_column_config_CellValueChanged);
            list_column_config.CurrentCellDirtyStateChanged += new EventHandler(list_column_config_CurrentCellDirtyStateChanged);
        }

        // This event handler manually raises the CellValueChanged event by calling the CommitEdit method. 
        private void list_column_config_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (list_column_config.IsCurrentCellDirty && list_column_config.CurrentCell.ColumnIndex == 0)
            {
                list_column_config.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void list_column_config_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                // This check is needed because the method will be called whenever a cell value is changed 
                var columnComboBox = (DataGridViewComboBoxCell)list_column_config.Rows[e.RowIndex].Cells[1];
                columnComboBox.Value = null;
                var selectedTable = list_column_config.CurrentCell.Value.ToString();
                setupColumnsComboBox(selectedTable);
            }
        }

        private void button_table_config_Click(object sender, EventArgs e)
        {
            StatementsFormTables statementsFormTables = new StatementsFormTables(tcpClient, databaseName, selectedTables);
            statementsFormTables.ShowDialog();
            selectedTables = statementsFormTables.getSelectedTables();
            changeLayoutAfterTreatySelection();
        }

        private void changeLayoutAfterTreatySelection()
        {
            setupTablesComboBox();
            getValuesForColumnsComboBox();
        }

        private void setupTablesComboBox()
        {
            binding_source_table_name.DataSource = selectedTables;
        }

        private void setupColumnsComboBox(string tableName)
        {
            binding_source_column_name.DataSource = tableColumns.Find(elem => elem.Key == tableName).Value;
        }

        private void getValuesForColumnsComboBox()
        { 
            foreach (var table in selectedTables)
            {
                var columnsInTable = new List<string>();
                // Command CREATE_INDEX just returns the column names from the table :)))) 
                tcpClient.Write(Commands.CREATE_INDEX + ";" + databaseName + ";" + table);
                var columnNames = tcpClient.ReadFromServer().Split(';')[1].Split('|');

                foreach (var column in columnNames)
                {
                    columnsInTable.Add(column);
                }
                tableColumns.Add(new KeyValuePair<string, List<string>>(table, columnsInTable));
            }
        }
    }
}
