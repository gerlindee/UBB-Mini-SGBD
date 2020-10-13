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
    public partial class MainForm : Form
    {
        private readonly Client tcpClient;
        private TreeNode rootNode = new TreeNode("Databases");
        private static string DELETE_DATABASE = "Delete Database";
        private static string CREATE_TABLE = "Create Table";
        private static string DELETE_TABLE = "Delete Table";

        private static string selectedDatabase = "";
        private static string selectedTable = "";

        MenuItem deleteTableMenuItem = new MenuItem(DELETE_TABLE);
        ContextMenu cm3 = new ContextMenu();

        MenuItem deleteDBMenuItem = new MenuItem(DELETE_DATABASE);
        MenuItem createTBMenuItem = new MenuItem(CREATE_TABLE);
        ContextMenu cm2 = new ContextMenu();

        public MainForm(Client client)
        {
            tcpClient = client;
            tcpClient.Connect();
            InitializeComponent();

            cm3.MenuItems.Add(deleteTableMenuItem);
            cm2.MenuItems.Add(deleteDBMenuItem);
            cm2.MenuItems.Add(createTBMenuItem);
            deleteDBMenuItem.Click += new EventHandler(contextMenu_deleteDB);
            deleteTableMenuItem.Click += new EventHandler(contextMenu_deleteTB);

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
            catch (Exception) 
            {
                // No databases have been added yet 
            }
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
            catch (Exception)
            {
                // No tables have been added yet 
            }
        }

        private void databasesList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && databasesList.FocusedItem.Bounds.Contains(e.Location))
            {
               cm2.Show(databasesList, e.Location);
            }
            else if (e.Button == MouseButtons.Left && databasesList.FocusedItem.Bounds.Contains(e.Location))
            {
                addTable_btn.Visible = true;
                selectedDatabase = databasesList.FocusedItem.Text;
                populateTables();
            }
        }

        private void tablesList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && tablesList.FocusedItem.Bounds.Contains(e.Location))
            {
                cm3.Show(tablesList, e.Location);
            }
            else if (e.Button == MouseButtons.Left && tablesList.FocusedItem.Bounds.Contains(e.Location))
            {
                // get the columns for the table 
                selectedTable = tablesList.FocusedItem.Text;

            }
        }

        private void addDB_buttonClick(object sender, EventArgs e)
        {
            AddDatabaseForm createDBForm = new AddDatabaseForm(tcpClient);
            createDBForm.ShowDialog(this);
            var serverResponse = tcpClient.ReadFromServer();	            
            MessageBox.Show(serverResponse, "Execution result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            populateDatabases();
        }

        private void contextMenu_deleteDB(object sender, EventArgs e)
        {
            var selectedDBName = databasesList.FocusedItem.Text;
            tcpClient.Write(Commands.DROP_DATABASE + ";" + selectedDBName);
            var serverResponse = tcpClient.ReadFromServer();
            MessageBox.Show(serverResponse, "Execution result", MessageBoxButtons.OK, MessageBoxIcon.Information);

            populateDatabases();
        }

        private void contextMenu_deleteTB(object sender, EventArgs e)
        {
            var selectedTBName = tablesList.FocusedItem.Text;
            tcpClient.Write(Commands.DROP_TABLE + ';' + selectedDatabase +";" + selectedTBName);
            var serverResponse = tcpClient.ReadFromServer();
            MessageBox.Show(serverResponse, "Execution result", MessageBoxButtons.OK, MessageBoxIcon.Information);

            populateTables();
        }

        private void addTB_Click(object sender, EventArgs e)
        {
            TestFormTables addTableForm = new TestFormTables(selectedDatabase, tcpClient);
            addTableForm.ShowDialog(this);
            // var serverResponse = tcpClient.ReadFromServer();
            // MessageBox.Show(serverResponse, "Execution result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            populateTables();
        }
    }
}
