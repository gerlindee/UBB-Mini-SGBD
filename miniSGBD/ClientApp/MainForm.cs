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
        private static string CREATE_DATABASE = "Create Database";
        private static string DELETE_DATABASE = "Delete Database";
        private static string CREATE_TABLE = "Create Table";
        private static string DELETE_TABLE = "Deelete Table";

        MenuItem createDBMenuItem = new MenuItem(CREATE_DATABASE);
        ContextMenu cm = new ContextMenu();

        MenuItem deleteDBMenuItem = new MenuItem(DELETE_DATABASE);
        MenuItem createTBMenuItem = new MenuItem(CREATE_TABLE);
        ContextMenu cm2 = new ContextMenu();

        public MainForm(Client client)
        {
            tcpClient = client;
            tcpClient.Connect();
            InitializeComponent();

            cm.MenuItems.Add(createDBMenuItem);
            cm2.MenuItems.Add(deleteDBMenuItem);
            cm2.MenuItems.Add(createTBMenuItem);
            deleteDBMenuItem.Click += new EventHandler(contextMenu_deleteDB);

            populateDatabases();
        }

        private void populateDatabases()
        {
            databasesList.Clear();
            tcpClient.Write(Commands.GET_ALL_DATABASES + ";");
            var serverResponse = tcpClient.ReadFromServer();
            //string message = Responses.MapResponseToMessage(serverResponse);

            var commandSplit = serverResponse.Split(';');
            var databasesNames = commandSplit[1].Split('|');

            foreach (var dbName in databasesNames)
                databasesList.Items.Add(dbName);
        }

        private void populateTables()
        {

        }

        private void databasesList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && databasesList.FocusedItem.Bounds.Contains(e.Location))
            {
               cm2.Show(databasesList, e.Location);
            }
            else if (e.Button == MouseButtons.Left && databasesList.FocusedItem.Bounds.Contains(e.Location))
            {
                populateTables();
            }
        }

        private void addDB_buttonClick(object sender, EventArgs e)
        {
            AddDatabaseForm createDBForm = new AddDatabaseForm(tcpClient);
            createDBForm.ShowDialog(this);
            populateDatabases();
        }

        private void contextMenu_deleteDB(object sender, EventArgs e)
        {
            var selectedDBName = databasesList.FocusedItem.Text;
            tcpClient.Write(Commands.DROP_DATABASE + ";" + selectedDBName);
            var serverResponse = tcpClient.ReadFromServer();
            populateDatabases();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
