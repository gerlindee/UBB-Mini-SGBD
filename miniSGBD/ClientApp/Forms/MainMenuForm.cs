﻿using ClientApp;
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
        private static string INSERT_TABLE = "Insert in table";

        private static string selectedDatabase = "";
        private static string selectedTable = "";

        MenuItem deleteTableMenuItem = new MenuItem(DELETE_TABLE);
        MenuItem createIndexMenuItem = new MenuItem(CREATE_INDEX);
        MenuItem insertTableMenuItem = new MenuItem(INSERT_TABLE);
        ContextMenu cm3 = new ContextMenu();

        MenuItem deleteDBMenuItem = new MenuItem(DELETE_DATABASE);
        MenuItem createTBMenuItem = new MenuItem(CREATE_TABLE);
        ContextMenu cm2 = new ContextMenu();

        public MainMenuForm(Client client)
        {
            tcpClient = client;
            tcpClient.Connect();
            InitializeComponent();

            cm3.MenuItems.Add(insertTableMenuItem);
            cm3.MenuItems.Add(deleteTableMenuItem);
            cm3.MenuItems.Add(createIndexMenuItem);
            cm2.MenuItems.Add(deleteDBMenuItem);
            cm2.MenuItems.Add(createTBMenuItem);
            insertTableMenuItem.Click += new EventHandler(contextMenu_insertTable);
            deleteDBMenuItem.Click += new EventHandler(contextMenu_deleteDB);
            deleteTableMenuItem.Click += new EventHandler(contextMenu_deleteTB);
            createIndexMenuItem.Click += new EventHandler(contextMenu_createIN);
            createTBMenuItem.Click += new EventHandler(addTB_Click);
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
            if (e.Button == MouseButtons.Right && databasesList.FocusedItem.Bounds.Contains(e.Location))
            {
               cm2.Show(databasesList, e.Location);
            }
            else if (e.Button == MouseButtons.Left && databasesList.FocusedItem.Bounds.Contains(e.Location))
            {
                table_structure_list.Clear();
                table_contents_list.DataSource = null;
                table_contents_list.Rows.Clear();
                addTable_btn.Visible = true;
                selectedDatabase = databasesList.FocusedItem.Text;
                populateTables();
            }
        }

        private void tablesList_MouseClick(object sender, MouseEventArgs e)
        {
            selectedTable = tablesList.FocusedItem.Text;

            table_structure_list.Clear();
            table_contents_list.DataSource = null;
            table_contents_list.Rows.Clear();
            var recordsDataTable = new DataTable(); // for displaying the DataGridView of the table contents 

            // Display the info about the structure of the clicked table
            tcpClient.Write(Commands.GET_TABLE_INFORMATION + ";" + selectedDatabase + ";" + selectedTable);
            var serverResponse = tcpClient.ReadFromServer().Split(';');
            var retreivedInformation = serverResponse[1].Split('|');
            foreach (string tableInfo in retreivedInformation)
            {
                table_structure_list.Items.Add(tableInfo);
                recordsDataTable.Columns.Add(tableInfo.Split(':')[0]);
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
                        var row = recordsDataTable.NewRow();
                        for (int idx = 0; idx < tableRecordSplit.Length; idx++)
                        {
                            row[idx] = tableRecordSplit[idx];
                        }
                        recordsDataTable.Rows.Add(row);
                    }
                }
                table_contents_list.DataSource = recordsDataTable;
            }
            else
            {
                MessageBox.Show(serverResponse[0], "Query Execution Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
            MessageBox.Show(serverResponse, "Query Execution Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            populateDatabases();
        }

        private void contextMenu_deleteTB(object sender, EventArgs e)
        {
            var selectedTBName = tablesList.FocusedItem.Text;
            tcpClient.Write(Commands.DROP_TABLE + ';' + selectedDatabase +";" + selectedTBName);
            var serverResponse = tcpClient.ReadFromServer();
            MessageBox.Show(serverResponse, "Execution result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            populateTables();
            table_structure_list.Clear();
            table_contents_list.DataSource = null;
            table_contents_list.Rows.Clear();
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
        }

        private void contextMenu_insertRecords(object sender, EventArgs e)
        {
            tcpClient.Write(Commands.INSERT_INTO_TABLE + ';' + selectedDatabase + ';' + selectedTable + ";hello|me");
            var serverResponse = tcpClient.ReadFromServer();
            MessageBox.Show(serverResponse, "Execution result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void addTB_Click(object sender, EventArgs e)
        {
            CreateTableForm addTableForm = new CreateTableForm(selectedDatabase, tcpClient);
            addTableForm.ShowDialog(this);
            populateTables();
        }
    }
}
