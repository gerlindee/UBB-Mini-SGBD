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
    public partial class SelectForm : Form
    {
        private string databaseName;
        private Client tcpClient;
        private List<string> selectedTables;
        private List<KeyValuePair<string, List<string>>> tableColumns;
        private List<Tuple<string, CheckedListBox>> checkedForeignKeyColumns;
        private List<string> tableJoinDetails; 
        private bool AtLeastOneOutputSelected; // used for validation 

        public SelectForm(string _databaseName, Client _tcpClient)
        {
            databaseName = _databaseName;
            tcpClient = _tcpClient;
            selectedTables = new List<string>();
            tableColumns = new List<KeyValuePair<string, List<string>>>();
            checkedForeignKeyColumns = new List<Tuple<string, CheckedListBox>>();
            tableJoinDetails = new List<string>();
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
            SelectFormTables statementsFormTables = new SelectFormTables(tcpClient, databaseName, selectedTables);
            statementsFormTables.ShowDialog();
            selectedTables = statementsFormTables.getSelectedTables();
            changeLayoutAfterTableSelection();
        }

        private void changeLayoutAfterTableSelection()
        {
            panel_join_config.Controls.Clear();
            list_column_config.Rows.Clear();
            setupTablesComboBox();
            getValuesForColumnsComboBox();
            setUpJoinTables();

            if (selectedTables.Count > 1)
            {
                checkForeignKeyColumns();
            }
        }

        private void setUpJoinTables()
        {
            foreach (var table in selectedTables)
            {
                var panelForTable = new FlowLayoutPanel
                {
                    AutoSize = true,
                    FlowDirection = FlowDirection.TopDown
                };

                var tableName = new Label { Text = table };

                var columnsCheckBoxed = new CheckedListBox
                {
                    Enabled = false
                };

                foreach (var column in tableColumns.Find(elem => elem.Key == table).Value)
                {
                    columnsCheckBoxed.Items.Add(column);
                }

                panelForTable.Controls.Add(tableName);
                panelForTable.Controls.Add(columnsCheckBoxed);
                panel_join_config.Controls.Add(panelForTable);

                checkedForeignKeyColumns.Add(new Tuple<string, CheckedListBox>(table, columnsCheckBoxed));
            }
        }

        private void checkForeignKeyColumns()
        {
            foreach (var table in checkedForeignKeyColumns)
            {
                tcpClient.Write(Commands.GET_TABLE_FOREIGN_KEYS + ";" + databaseName + ";" + table.Item1);
                var serverResponse = tcpClient.ReadFromServer().Split(';');

                foreach (var foreignKey in serverResponse)
                {
                    if (foreignKey != "")
                    {
                        var fkInfo = foreignKey.Split('|');
                        var refTableCheckboxes = checkedForeignKeyColumns.Find(elem => elem.Item1 == fkInfo[0]);
                        if (selectedTables.Contains(fkInfo[0]))
                        {
                            for (int i = 1; i < fkInfo.Length; i++)
                            {
                                // Set the FK column to checked in the table where the reference is made
                                var idxInCheckList = table.Item2.Items.IndexOf(fkInfo[i]);
                                table.Item2.SetItemChecked(idxInCheckList, true);

                                // Set the corresponding PK column to checked in the referenced table
                                idxInCheckList = refTableCheckboxes.Item2.Items.IndexOf(fkInfo[i]);
                                refTableCheckboxes.Item2.SetItemChecked(idxInCheckList, true);

                                // Add the detais about the join conditions which will be sent to the server 
                                tableJoinDetails.Add(fkInfo[0] + "#" + table.Item1 + "#" + "FK_" + table.Item1 + "_" + fkInfo[0] + "#" + fkInfo[i]);
                            }
                        }
                    }
                }
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
                    tcpClient.Write(Commands.SELECT_RECORDS + ";" + databaseName + ";" + "SELECT_ALL#" + selectedTables[0]);
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
                        SelectResultForm statementsResultForm = new SelectResultForm(resultTableHeader, resultTableContents);
                        statementsResultForm.Show();
                    }
                    else
                    {
                        MessageBox.Show(serverResponse[0], "Query Execution Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    AtLeastOneOutputSelected = false; 

                    if (selectedTables.Count == 1)
                    {
                        // Select on one table, without Join => separate because no validations on join configuration is needed 
                        var message = Commands.SELECT_RECORDS + ';' + databaseName + ';';
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
                                message += tableName.ToString() + '#';

                                var columnName = (columnConfig[1] as DataGridViewComboBoxCell).Value;
                                if (columnName == null)
                                {
                                    MessageBox.Show("Column name needs to be selected in row " + idx.ToString(), "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    message += columnName.ToString() + '#';

                                    var aggregateFunction = (columnConfig[2] as DataGridViewComboBoxCell).Value;
                                    if (aggregateFunction == null)
                                    {
                                        message += "-#";
                                    }
                                    else
                                    {
                                        message += aggregateFunction.ToString() + '#';
                                    }

                                    var aliasName = (columnConfig[3] as DataGridViewTextBoxCell).Value;
                                    if (aliasName == null)
                                    {
                                        message += "-#";
                                    }
                                    else
                                    {
                                        message += aliasName.ToString() + '#';
                                    }

                                    var outputCheck = (columnConfig[4] as DataGridViewCheckBoxCell).Value;
                                    if (outputCheck == null)
                                    {
                                        message += "-#";
                                    }
                                    else
                                    {
                                        AtLeastOneOutputSelected = true;
                                        message += SelectColumnInformation.Output + '#';
                                    }

                                    var filterValue = (columnConfig[5] as DataGridViewTextBoxCell).Value;
                                    if (filterValue == null)
                                    {
                                        message += "-#";
                                    }
                                    else
                                    {
                                        message += filterValue.ToString() + '#';
                                    }

                                    var groupByCheck = (columnConfig[6] as DataGridViewCheckBoxCell).Value;
                                    if (groupByCheck == null)
                                    {
                                        message += "-#";
                                    }
                                    else
                                    {
                                        message += SelectColumnInformation.GroupBy + '#';
                                    }

                                    var havingValue = (columnConfig[7] as DataGridViewTextBoxCell).Value;
                                    if (havingValue == null)
                                    {
                                        message += "-|";
                                    }
                                    else
                                    {
                                        message += havingValue.ToString() + '|';
                                    }
                                }
                            }
                        }

                        if (!AtLeastOneOutputSelected)
                        {
                            MessageBox.Show("At least one column needs to be selected for output!", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            tcpClient.Write(message);
                            var serverResponse = tcpClient.ReadFromServer().Split(';');

                            if (serverResponse[0] == Commands.MapCommandToSuccessResponse(Commands.SELECT_RECORDS))
                            {
                                var resultTableHeader = new List<string>();
                                foreach (var outHeader in serverResponse[1].Split('#'))
                                {
                                    resultTableHeader.Add(outHeader);
                                }

                                var resultTableContents = new List<string>();
                                foreach (var outRecord in serverResponse[2].Split('|'))
                                {
                                    resultTableContents.Add(outRecord);
                                }

                                SelectResultForm statementsResultForm = new SelectResultForm(resultTableHeader, resultTableContents);
                                statementsResultForm.Show();
                            }
                            else
                            {
                                MessageBox.Show(serverResponse[0], "Query Execution Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        // Build the list of tables which have at least one FK checked for the JOIN operation 
                        var tablesWithCheckedFK = new List<string>();
                        foreach (FlowLayoutPanel tableControl in panel_join_config.Controls)
                        {
                            var tableName = tableControl.Controls[0].Text;
                            var checkedColumns = (tableControl.Controls[1] as CheckedListBox).CheckedItems;
                            if (checkedColumns.Count != 0)
                            {
                                tablesWithCheckedFK.Add(tableName);
                            }
                        }

                        // If the number of tables with checked columns differs from the total number of tables selected => between two tables no FK relation was found => error 
                        if (tablesWithCheckedFK.Count != selectedTables.Count)
                        {
                            MessageBox.Show("There is no Foreign Key relation between the selected tables, JOIN operation cannot be performed!", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            var message = Commands.SELECT_RECORDS_WITH_JOIN + ";" + databaseName + ";";

                            if (columnConditions == 1)
                            {
                                message += "SELECT_ALL|";

                                // SELECT * from the result of the join 
                                foreach (var selectedTable in selectedTables)
                                {
                                    var joinCondition = tableJoinDetails.Find(elem => elem.Split('#')[0] == selectedTable);
                                    
                                    if (joinCondition != null)
                                    {
                                        message += joinCondition + "|";
                                    }
                                }
                                message = message.Remove(message.Length - 1);

                                tcpClient.Write(message);
                                var serverResponse = tcpClient.ReadFromServer().Split(';');

                                if (serverResponse[0] == Commands.MapCommandToSuccessResponse(Commands.SELECT_RECORDS_WITH_JOIN))
                                {
                                    var resultTableHeader = new List<string>();
                                    foreach (var outHeader in serverResponse[1].Split('#'))
                                    {
                                        resultTableHeader.Add(outHeader);
                                    } 

                                    var resultTableContents = new List<string>();
                                    foreach (var outRecord in serverResponse[2].Split('|'))
                                    {
                                        resultTableContents.Add(outRecord);
                                    }

                                    SelectResultForm statementsResultForm = new SelectResultForm(resultTableHeader, resultTableContents);
                                    statementsResultForm.Show();
                                }
                                else
                                {
                                    MessageBox.Show(serverResponse[0], "Query Execution Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                // SELECT with some output conditions on the results of the join 
                                foreach (var selectedTable in selectedTables)
                                {
                                    var joinCondition = tableJoinDetails.Find(elem => elem.Split('#')[0] == selectedTable);

                                    if (joinCondition != null)
                                    {
                                        message += joinCondition + "*";
                                    }
                                }
                                message = message.Remove(message.Length - 1) + "|";

                                var noRows = list_column_config.Rows.Count - 1;
                                AtLeastOneOutputSelected = false;

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
                                        message += tableName.ToString() + '#';

                                        var columnName = (columnConfig[1] as DataGridViewComboBoxCell).Value;
                                        if (columnName == null)
                                        {
                                            MessageBox.Show("Column name needs to be selected in row " + idx.ToString(), "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        else
                                        {
                                            message += columnName.ToString() + '#';

                                            var aggregateFunction = (columnConfig[2] as DataGridViewComboBoxCell).Value;
                                            if (aggregateFunction == null)
                                            {
                                                message += "-#";
                                            }
                                            else
                                            {
                                                message += aggregateFunction.ToString() + '#';
                                            }

                                            var aliasName = (columnConfig[3] as DataGridViewTextBoxCell).Value;
                                            if (aliasName == null)
                                            {
                                                message += "-#";
                                            }
                                            else
                                            {
                                                message += aliasName.ToString() + '#';
                                            }

                                            var outputCheck = (columnConfig[4] as DataGridViewCheckBoxCell).Value;
                                            if (outputCheck == null)
                                            {
                                                message += "-#";
                                            }
                                            else
                                            {
                                                AtLeastOneOutputSelected = true;
                                                message += SelectColumnInformation.Output + '#';
                                            }

                                            var filterValue = (columnConfig[5] as DataGridViewTextBoxCell).Value;
                                            if (filterValue == null)
                                            {
                                                message += "-#";
                                            }
                                            else
                                            {
                                                message += filterValue.ToString() + '#';
                                            }

                                            var groupByCheck = (columnConfig[6] as DataGridViewCheckBoxCell).Value;
                                            if (groupByCheck == null)
                                            {
                                                message += "-#";
                                            }
                                            else
                                            {
                                                message += SelectColumnInformation.GroupBy + '#';
                                            }

                                            var havingValue = (columnConfig[7] as DataGridViewTextBoxCell).Value;
                                            if (havingValue == null)
                                            {
                                                message += "-*";
                                            }
                                            else
                                            {
                                                message += havingValue.ToString() + '*';
                                            }
                                        }
                                    }
                                }

                                if (!AtLeastOneOutputSelected)
                                {
                                    MessageBox.Show("At least one column needs to be selected for output!", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    tcpClient.Write(message);
                                    var serverResponse = tcpClient.ReadFromServer().Split(';');

                                    if (serverResponse[0] == Commands.MapCommandToSuccessResponse(Commands.SELECT_RECORDS))
                                    {
                                        var resultTableHeader = new List<string>();
                                        foreach (var outHeader in serverResponse[1].Split('#'))
                                        {
                                            resultTableHeader.Add(outHeader);
                                        }

                                        var resultTableContents = new List<string>();
                                        foreach (var outRecord in serverResponse[2].Split('|'))
                                        {
                                            resultTableContents.Add(outRecord);
                                        }

                                        SelectResultForm statementsResultForm = new SelectResultForm(resultTableHeader, resultTableContents);
                                        statementsResultForm.Show();
                                    }
                                    else
                                    {
                                        MessageBox.Show(serverResponse[0], "Query Execution Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
