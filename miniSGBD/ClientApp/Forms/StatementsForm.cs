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
        private List<string> selectedTables; 

        public StatementsForm(string _databaseName, Client _tcpClient)
        {
            databaseName = _databaseName;
            tcpClient = _tcpClient;
            selectedTables = new List<string>();
            InitializeComponent();
        }

        private void button_table_config_Click(object sender, EventArgs e)
        {
            StatementsFormTables statementsFormTables = new StatementsFormTables(tcpClient, databaseName, selectedTables);
            statementsFormTables.ShowDialog();
            selectedTables = statementsFormTables.getSelectedTables();
        }
    }
}
