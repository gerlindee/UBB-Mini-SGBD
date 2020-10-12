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
    public partial class AddDatabaseForm : Form
    {
        private readonly Client tcpClient;
        public AddDatabaseForm(Client client)
        {
            tcpClient = client;
            InitializeComponent();
        }

        private void create_db_btn_Click(object sender, EventArgs e)
        {
            DisplayQueryResult(Commands.CREATE_DATABASE);
        }

        private void cancel_db_btn_Click(object sender, EventArgs e)
        {
            this.Close();
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
                message = tcpClient.ReadFromServer();
                caption = "Query Execution Result";
                if (message != Commands.MapCommandToSuccessResponse(action))
                    type = MessageBoxIcon.Error;
                else
                    type = MessageBoxIcon.Information;
            }
            MessageBox.Show(message, caption, MessageBoxButtons.OK, type);
            this.Close();
        }
    }
}
