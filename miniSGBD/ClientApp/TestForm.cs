using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class TestForm : Form
    {
        private Client Client; 

        public TestForm(Client client)
        {
            Client = client;
            Client.Connect();
            InitializeComponent();
        }

        private void button_db_name_Click(object sender, EventArgs e)
        {
            string Message = text_db_name.Text;
            Client.WriteToConnection(Message);
        }
    }
}
