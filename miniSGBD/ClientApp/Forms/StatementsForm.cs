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

namespace miniSGBD.Forms
{
    public partial class StatementsForm : Form
    {
        private string databaseName;
        private Client tcpClient;

        public StatementsForm(string _databaseName, Client _tcpClient)
        {
            databaseName = _databaseName;
            tcpClient = _tcpClient;
            InitializeComponent();
        }
    }
}
