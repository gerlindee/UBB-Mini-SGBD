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
            DatabaseAction(Commands.CREATE_DATABASE);
        }

        private void button_db_delete_Click(object sender, EventArgs e)
        {
            DatabaseAction(Commands.DROP_DATABASE);
        }

        private void DatabaseAction(string action)
        {
            var databaseName = text_db_name.Text;
            string message;
            string caption;

            if (databaseName == "")
            {
                message = "Database name cannot be empty!";
                caption = "Validation Error";
            }
            else
            {
                tcpClient.Write(action + ";" + databaseName);
                message = Responses.MapResponseToMessage(tcpClient.ReadFromServer());
                caption = "Query Execution Result";
            }
            MessageBox.Show(message, caption, MessageBoxButtons.OK);
        }
    }
}
