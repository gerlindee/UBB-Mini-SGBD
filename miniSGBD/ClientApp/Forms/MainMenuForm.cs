using ClientApp;
using miniSGBD.Forms;
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
    public partial class MainMenuForm : Form
    {
        private readonly Client tcpClient;
        private TreeNode rootNode = new TreeNode("Databases");
        private static string DELETE_DATABASE = "Delete Database";
        private static string CREATE_TABLE = "Create Table";
        private static string DELETE_TABLE = "Delete Table";
        private static string CREATE_INDEX = "Create Index";
        private static string INSERT_TABLE = "Insert Records";
        private static string DELETE_RECORD = "Delete record";
        private static string CREATE_STATEMENT = "Create Selection Query";

        private static string selectedDatabase = "";
        private static string selectedTable = "";

        MenuItem deleteTableMenuItem = new MenuItem(DELETE_TABLE);
        MenuItem createIndexMenuItem = new MenuItem(CREATE_INDEX);
        MenuItem insertTableMenuItem = new MenuItem(INSERT_TABLE);
        ContextMenu cm3 = new ContextMenu();

        MenuItem deleteDBMenuItem = new MenuItem(DELETE_DATABASE);
        MenuItem createTBMenuItem = new MenuItem(CREATE_TABLE);
        MenuItem createStatementMenuItem = new MenuItem(CREATE_STATEMENT);
        ContextMenu cm2 = new ContextMenu();

        MenuItem deleteRecordMenuItem = new MenuItem(DELETE_RECORD);
        ContextMenu cm1 = new ContextMenu();

        private List<ColumnInfo> columnInfoList = new List<ColumnInfo>();
        private int selectedRowToDelete = -1;

        public MainMenuForm(Client client)
        {
            tcpClient = client;
            tcpClient.Connect();
            InitializeComponent();

            cm1.MenuItems.Add(deleteRecordMenuItem);
            cm3.MenuItems.Add(insertTableMenuItem);
            cm3.MenuItems.Add(deleteTableMenuItem);
            cm3.MenuItems.Add(createIndexMenuItem);
            cm2.MenuItems.Add(createStatementMenuItem);
            cm2.MenuItems.Add(deleteDBMenuItem);
            cm2.MenuItems.Add(createTBMenuItem);
            insertTableMenuItem.Click += new EventHandler(contextMenu_insertTable);
            deleteDBMenuItem.Click += new EventHandler(contextMenu_deleteDB);
            deleteTableMenuItem.Click += new EventHandler(contextMenu_deleteTB);
            createIndexMenuItem.Click += new EventHandler(contextMenu_createIN);
            createTBMenuItem.Click += new EventHandler(contextMenu_addTable);
            deleteRecordMenuItem.Click += new EventHandler(contextMenu_deleteRecord);
            createStatementMenuItem.Click += new EventHandler(contextMenu_createStatement);

            addTable_btn.Visible = false;

            populateDatabases();
        }

        private void populateDatabases()
        {
            databasesList.Clear();
            tcpClient.Write(Commands.GET_ALL_DATABASES + ';');
            var serverResponse = tcpClient.ReadFromServer();

            var commandSplit = serverResponse.Split(';');

            try
            {
                var databasesNames = commandSplit[1].Split('|');
                foreach (var dbName in databasesNames)
                    databasesList.Items.Add(dbName);
            }
            catch (Exception) {}
        }

        private void populateTables()
        {
            tablesList.Clear();
            tcpClient.Write(Commands.GET_ALL_TABLES + ';' + selectedDatabase);
            var serverResponse = tcpClient.ReadFromServer();
            var commandSplit = serverResponse.Split(';');

            try
            {
                var tableNames = commandSplit[1].Split('|');
                foreach (var tName in tableNames)
                    tablesList.Items.Add(tName);
            }
            catch (Exception) {}
        }

        private void databasesList_MouseClick(object sender, MouseEventArgs e)
        {
            selectedDatabase = databasesList.FocusedItem.Text;
            if (e.Button == MouseButtons.Right && databasesList.FocusedItem.Bounds.Contains(e.Location))
            {
               cm2.Show(databasesList, e.Location);
            }
            else if (e.Button == MouseButtons.Left && databasesList.FocusedItem.Bounds.Contains(e.Location))
            {
                table_structure_list.Clear();
                table_contents_list.DataSource = null;
                table_contents_list.Rows.Clear();
                table_contents_list.Columns.Clear();

                addTable_btn.Visible = true;
                populateTables();
            }
        }

        private void buildTableInformationDisplays()
        {
            table_structure_list.Clear();
            table_contents_list.DataSource = null;
            table_contents_list.Rows.Clear();
            table_contents_list.Columns.Clear();
            columnInfoList.Clear();

            tcpClient.Write(Commands.GET_TABLE_INFORMATION + ";" + selectedDatabase + ";" + selectedTable);
            var serverResponse = tcpClient.ReadFromServer().Split(';');
            var columnsInfo = serverResponse[1].Split('|');
            foreach (var column in columnsInfo)
            {
                table_structure_list.Items.Add(column);
            }

            tcpClient.Write(Commands.GET_TABLE_COLUMNS + ";" + selectedDatabase + ";" + selectedTable);
            serverResponse = tcpClient.ReadFromServer().Split(';');
            var retreivedInformation = serverResponse[1].Split('|');

            foreach (var columnInfo in retreivedInformation)
            {
                columnInfoList.Add(new ColumnInfo(columnInfo)); // we have info about all columns
            }

            for (var i = 0; i < columnInfoList.Count; i++)
            {
                table_contents_list.Columns.Add(string.Format("col{0}", i), columnInfoList[i].ColumnName);
            }

            // Display the records for the clicked table
            tcpClient.Write(Commands.SELECT_RECORDS + ";" + selectedDatabase + ";" + selectedTable);
            serverResponse = tcpClient.ReadFromServer().Split(';');
            if (serverResponse[0] == Commands.MapCommandToSuccessResponse(Commands.SELECT_RECORDS))
            {
                var retrievedRecords = serverResponse[1].Split('|');
                foreach (string tableRecord in retrievedRecords)
                {
                    if (tableRecord != "")
                    {
                        var tableRecordSplit = tableRecord.Split('#');

                        int rowIndex = table_contents_list.Rows.Add();
                        var row = table_contents_list.Rows[rowIndex];

                        // Special handling for tables with one column 
                        if (columnInfoList.Count == 1)
                        {
                            row.Cells[0].Value = tableRecordSplit[0];
                        }
                        else
                        {
                            for (int idx = 0; idx < tableRecordSplit.Length; idx++)
                            {
                                row.Cells[idx].Value = tableRecordSplit[idx];
                            }
                        }                     
                    }
                }
            }
            else
            {
                MessageBox.Show(serverResponse[0], "Query Execution Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tablesList_MouseClick(object sender, MouseEventArgs e)
        {
            selectedTable = tablesList.FocusedItem.Text;

            buildTableInformationDisplays();

            if (e.Button == MouseButtons.Right && tablesList.FocusedItem.Bounds.Contains(e.Location))
            { 
                cm3.Show(tablesList, e.Location);
            }
        }

        private void addDB_buttonClick(object sender, EventArgs e)
        {
            CreateDatabaseForm createDBForm = new CreateDatabaseForm(tcpClient);
            createDBForm.ShowDialog(this);
            populateDatabases();
        }

        private void contextMenu_deleteDB(object sender, EventArgs e)
        {
            var selectedDBName = databasesList.FocusedItem.Text;
            tcpClient.Write(Commands.DROP_DATABASE + ";" + selectedDBName);
            var serverResponse = tcpClient.ReadFromServer();
            if (serverResponse == Commands.MapCommandToSuccessResponse(Commands.DROP_DATABASE))
            {
                MessageBox.Show(serverResponse, "Query Execution Result", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear the table information area just in case a table is selected when we delete a DB 
                tablesList.Clear();
                table_structure_list.Clear();
                table_contents_list.DataSource = null;
                table_contents_list.Rows.Clear();
                table_contents_list.Columns.Clear();
                columnInfoList.Clear();

                // Repopulate databases list 
                populateDatabases();
            }
            else
            {
                MessageBox.Show(serverResponse, "Query Execution Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void contextMenu_deleteTB(object sender, EventArgs e)
        { 
            var selectedTBName = tablesList.FocusedItem.Text;
            tcpClient.Write(Commands.DROP_TABLE + ';' + selectedDatabase +";" + selectedTBName);
            var serverResponse = tcpClient.ReadFromServer();

            if (Commands.MapCommandToSuccessResponse(Commands.DROP_TABLE) == serverResponse)
            {
                MessageBox.Show(serverResponse, "Execution result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                populateTables();
                table_structure_list.Clear();
                table_contents_list.DataSource = null;
                table_contents_list.Rows.Clear();
                table_contents_list.Columns.Clear();
            }
            else
            {
                MessageBox.Show(serverResponse, "Execution result", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void contextMenu_createIN(object sender, EventArgs e)
        {
            CreateIndexForm createDBForm = new CreateIndexForm(tcpClient, selectedDatabase, selectedTable);
            createDBForm.ShowDialog(this);
        }
        
        private void contextMenu_insertTable(object sender, EventArgs e)
        {
            InsertForm insertTableForm = new InsertForm(tcpClient, selectedDatabase, selectedTable);
            insertTableForm.ShowDialog(this);
            // refresh the records table after the InsertForm is closed
            buildTableInformationDisplays();
        }

        private void contextMenu_insertRecords(object sender, EventArgs e)
        {
            tcpClient.Write(Commands.INSERT_INTO_TABLE + ';' + selectedDatabase + ';' + selectedTable + ";hello|me");
            var serverResponse = tcpClient.ReadFromServer();
            MessageBox.Show(serverResponse, "Execution result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void contextMenu_addTable(object sender, EventArgs e)
        {
            CreateTableForm addTableForm = new CreateTableForm(selectedDatabase, tcpClient);
            addTableForm.ShowDialog(this);
            populateTables();
        }

        private void contextMenu_createStatement(object sender, EventArgs e)
        {
            SelectForm createStmForm = new SelectForm(selectedDatabase, tcpClient);
            createStmForm.ShowDialog(this);
        }

        private void contextMenu_deleteRecord(object sender, EventArgs e)
        {
            string messsage = Commands.DELETE_RECORD + ";" + selectedDatabase + ";" + selectedTable + ";";
            if (selectedRowToDelete > -1)
            {
                var row = table_contents_list.Rows[selectedRowToDelete];
                for(var index =0; index< row.Cells.Count -1; index ++)
                {
                    var columnName = table_contents_list.Columns[index].HeaderText.ToString();
                    if (columnInfoList.Exists(x => x.ColumnName == columnName && x.PK))
                    {
                        var columnValue = row.Cells[index].Value.ToString();
                        messsage += row.Cells[index].Value.ToString() + '#';
                    }
                        
                }
            }

            tcpClient.Write(messsage.Remove(messsage.Length - 1));

            var serverResponse = tcpClient.ReadFromServer();
            if (serverResponse == Commands.MapCommandToSuccessResponse(Commands.DELETE_RECORD))
            {
                MessageBox.Show(serverResponse, "Execution result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                table_contents_list.Rows.RemoveAt(selectedRowToDelete);
                selectedRowToDelete = -1;
            }
            else
            {
                MessageBox.Show(serverResponse, "Execution result", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                selectedRowToDelete = table_contents_list.HitTest(e.X, e.Y).RowIndex;
                if (selectedRowToDelete >= 0) //oriunde in tabel
                {
                    cm1.Show(table_contents_list, new Point(e.X, e.Y));

                }
            }
        }
    }
}
