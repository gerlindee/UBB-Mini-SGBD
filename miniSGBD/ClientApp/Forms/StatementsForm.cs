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
            panel_join_config.Controls.Clear();
            list_column_config.Rows.Clear();
            setupTablesComboBox();
            getValuesForColumnsComboBox();
            setUpJoinTables();
            setupSortingComboBox();
        }

        private void setUpJoinTables()
        {
            foreach (var table in selectedTables)
            {
                var panelForTable = new FlowLayoutPanel();
                panelForTable.AutoSize = true;
                panelForTable.FlowDirection = FlowDirection.TopDown;

                var tableName = new Label();
                tableName.Text = table;
                
                var columnsCheckBoxed = new CheckedListBox();
                foreach (var column in tableColumns.Find(elem => elem.Key == table).Value)
                {
                    columnsCheckBoxed.Items.Add(column);
                }

                panelForTable.Controls.Add(tableName);
                panelForTable.Controls.Add(columnsCheckBoxed);
                panel_join_config.Controls.Add(panelForTable);
            }

        }

        private void setupTablesComboBox()
        {
            binding_source_table_name.DataSource = selectedTables;
        }

        private void setupColumnsComboBox(string tableName)
        {
            binding_source_column_name.DataSource = tableColumns.Find(elem => elem.Key == tableName).Value;
        }

        private void setupSortingComboBox()
        {
            var sortingTypes = new List<string>();
            sortingTypes.Add("None");
            sortingTypes.Add("Ascending");
            sortingTypes.Add("Descending");
            binding_source_sorting.DataSource = sortingTypes;
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

        private void button_column_config_Click(object sender, EventArgs e)
        {
            if (selectedTables.Count == 0)
            {
                MessageBox.Show("At least one table needs to be selected!", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var columnConditions = list_column_config.Rows.Count;
                if (columnConditions == 1 && selectedTables.Count == 1)
                {
                    var resultTableContents = new List<string>();
                    // Select the entire contents of the table, unfiltered => SELECT * FROM <table>
                    tcpClient.Write(Commands.SELECT_RECORDS + ";" + databaseName + ";" + selectedTables[0]);
                    var serverResponse = tcpClient.ReadFromServer().Split(';');

                    if (serverResponse[0] == Commands.MapCommandToSuccessResponse(Commands.SELECT_RECORDS))
                    {
                        foreach (string tableRecord in serverResponse[1].Split('|'))
                        {
                            if (tableRecord != "")
                            {
                                resultTableContents.Add(tableRecord);
                            }
                        }

                        var resultTableHeader = tableColumns.Find(elem => elem.Key == selectedTables[0]).Value;
                        StatementsResultForm statementsResultForm = new StatementsResultForm(resultTableHeader, resultTableContents);
                        statementsResultForm.Show();
                    }
                    else
                    {
                        MessageBox.Show(serverResponse[0], "Query Execution Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else 
                {
                    if (selectedTables.Count == 1)
                    {
                        // Select on one table, without Join => separate because no validations on join configuration is needed 
                        var message = Commands.SELECT_QUERY + ';' + databaseName + ';' + selectedTables[0] + ';';
                        var noRows = list_column_config.Rows.Count - 1;

                        for (int idx = 0; idx < noRows; idx++)
                        {
                            var columnConfig = list_column_config.Rows[idx].Cells;
                            var tableName = (columnConfig[0] as DataGridViewComboBoxCell).Value;

                            if (tableName == null)
                            {
                                MessageBox.Show("Table name needs to be selected in row " + idx.ToString(), "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                var columnName = (columnConfig[1] as DataGridViewComboBoxCell).Value;
                                if (columnName == null)
                                {
                                    MessageBox.Show("Column name needs to be selected in row " + idx.ToString(), "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {

                                }
                            }

                        }
                    }
                    else
                    {
                        // Build the list of checked columns for join, for each displayed table
                        var selectedJoinColumns = new List<KeyValuePair<string, List<string>>>();
                        foreach (FlowLayoutPanel tableControl in panel_join_config.Controls)
                        {
                            var tableName = tableControl.Controls[0].Text;
                            var checkedColumns = (tableControl.Controls[1] as CheckedListBox).CheckedItems;
                            if (checkedColumns.Count != 0)
                            {
                                var joinColumns = new List<string>();
                                foreach (var column in checkedColumns)
                                {
                                    joinColumns.Add(column.ToString());
                                }
                                selectedJoinColumns.Add(new KeyValuePair<string, List<string>>(tableName, joinColumns));
                            }
                        }

                        // If there is a selected table for the join but no columns selected for the join => error 
                        if (selectedJoinColumns.Count != selectedTables.Count)
                        {
                            MessageBox.Show("Configure the columns for the join opereation for all selected tables!", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            // If the number of columns selected for join differs between tables => error 
                            var errorColumnsFlag = false;
                            for (int idx = 0; idx < selectedJoinColumns.Count - 1; idx++)
                            {
                                if (selectedJoinColumns[idx].Value.Count != selectedJoinColumns[idx + 1].Value.Count)
                                {
                                    errorColumnsFlag = true;
                                    break;
                                }
                            }

                            if (errorColumnsFlag)
                            {
                                MessageBox.Show("The same number of columns need to be selected for all join tables!", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                // Aici o sa fie chemat Select Query-ul cu Join-uri 
                            }
                        }
                    }
                }
            }
        }
    }
}
