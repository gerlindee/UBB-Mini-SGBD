using miniSGBD;
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

namespace ClientApp
{
    public partial class TestForm : Form
    {
        private readonly Client tcpClient; 

        public TestForm(Client client)
        {
            tcpClient = client;
            tcpClient.Connect();
            InitializeComponent();
        }

        private void button_db_name_Click(object sender, EventArgs e)
        {
            DisplayQueryResult(Commands.CREATE_DATABASE);
        }

        private void button_db_delete_Click(object sender, EventArgs e)
        {
            DisplayQueryResult(Commands.DROP_DATABASE);
        }

        private void DisplayQueryResult(string action)
        {
            var databaseName = text_db_name.Text;
            string message;
            string caption;
            MessageBoxIcon type;

            if (databaseName == "")
            {
                message = "Database name cannot be empty!";
                caption = "Validation Error";
                type = MessageBoxIcon.Exclamation;
            }
            else
            {
                tcpClient.Write(action + ";" + databaseName);
                var serverResponse = tcpClient.ReadFromServer();
                message = Responses.MapResponseToMessage(serverResponse);
                caption = "Query Execution Result";
                if (serverResponse != Commands.MapCommandToSuccessResponse(action))
                    type = MessageBoxIcon.Error;
                else
                    type = MessageBoxIcon.Information;


            }
            MessageBox.Show(message, caption, MessageBoxButtons.OK, type);
        }

        private void button_db_show_all_Click(object sender, EventArgs e)
        {
            tcpClient.Write(Commands.GET_ALL_DATABASES);
            var response = tcpClient.ReadFromServer().Split(';');
            MessageBox.Show(response[1], "All Database Names", MessageBoxButtons.OK);
        }

        private void button_db_show_all_tables_Click(object sender, EventArgs e)
        {
            // TODO: after create table is done   
        }

        private void button_db_create_table_nav_Click(object sender, EventArgs e)
        {
            var createTableForm = new TestFormTables(text_db_name.Text, tcpClient);
            createTableForm.Show();
        }
    }
}
