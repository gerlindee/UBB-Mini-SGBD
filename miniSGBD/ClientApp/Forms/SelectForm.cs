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

            list_join_config.CellValueChanged += new DataGridViewCellEventHandler(list_join_config_CellValueChanged);
            list_join_config.CurrentCellDirtyStateChanged += new EventHandler(list_join_config_CurrentCellDirtyStateChanged);

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

        private void list_join_config_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (list_join_config.IsCurrentCellDirty && (list_join_config.CurrentCell.ColumnIndex == 0 || list_join_config.CurrentCell.ColumnIndex == 2))
            {
                list_join_config.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void list_join_config_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var columnComboBox = (DataGridViewComboBoxCell)list_join_config.Rows[e.RowIndex].Cells[1];
                columnComboBox.Value = null;
                var selectedTable = list_join_config.CurrentCell.Value.ToString();
                setupColumnsComboBox(selectedTable);
            }

            if (e.ColumnIndex == 2)
            {
                var columnComboBox = (DataGridViewComboBoxCell)list_join_config.Rows[e.RowIndex].Cells[3];
                columnComboBox.Value = null;
                var selectedTable = list_join_config.CurrentCell.Value.ToString();
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
            list_column_config.Rows.Clear();
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
                        // Select on one table, without Join
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
                        var message = Commands.SELECT_RECORDS_WITH_JOIN + ';' + databaseName + ';';
                        var noRows = list_join_config.Rows.Count - 1;

                        for (int idx = 0; idx < noRows; idx++)
                        {
                            var joinConfig = list_join_config.Rows[idx].Cells;
                            var leftTableName = (joinConfig[0] as DataGridViewComboBoxCell).Value;
                            var leftTableColumn = (joinConfig[1] as DataGridViewComboBoxCell).Value;
                            var rightTableName = (joinConfig[2] as DataGridViewComboBoxCell).Value;
                            var rightTableColumn = (joinConfig[3] as DataGridViewComboBoxCell).Value;

                            message += leftTableName + "#" + leftTableColumn + "#" + rightTableName + "#" + rightTableColumn + "*";
                        }
                        message = message.Remove(message.Length - 1) + "|";

                        noRows = list_column_config.Rows.Count;
                        if (noRows > 1)
                        {
                            for (int idx = 0; idx < noRows - 1; idx++)
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
                        }

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
                }
            }
        }
    }
}
