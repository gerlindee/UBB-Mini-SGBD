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
            tcpClient.Write(Commands.CREATE_DATABASE + ";" + text_db_name.Text);
        }
    }
}
