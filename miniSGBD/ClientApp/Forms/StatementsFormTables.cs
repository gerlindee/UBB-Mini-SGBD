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

namespace miniSGBD.Forms
{
    public partial class StatementsFormTables : Form
    {
        private Client tcpClient;
        private List<string> selectedTables;
        private string databaseName; 

        public StatementsFormTables(Client _client, string _databaseName, List<string> _selectedTables)
        {
            tcpClient = _client;
            databaseName = _databaseName;
            selectedTables = _selectedTables;
            InitializeComponent();
            populateTablesList();
        }

        private void button_tables_Click(object sender, EventArgs e)
        {
            for (int idx = 0; idx < list_tables.Rows.Count; idx++)
            {
                try
                {
                    var isSelected = bool.Parse(list_tables.Rows[idx].Cells[1].Value.ToString());
                    var tableName = list_tables.Rows[idx].Cells[0].Value.ToString();
                    if (isSelected == true && !selectedTables.Contains(tableName))
                    {
                        selectedTables.Add(tableName);
                    }

                    if (isSelected == false && selectedTables.Contains(tableName))
                    {
                        selectedTables.Remove(tableName);
                    }
                }
                catch (Exception)
                {
                    // Null value for checkbox => it's not selected
                    var tableName = list_tables.Rows[idx].Cells[0].Value.ToString();
                    if (selectedTables.Contains(tableName))
                    {
                        selectedTables.Remove(tableName);
                    }
                }
                
            }
        }

        private void populateTablesList()
        {
            tcpClient.Write(Commands.GET_ALL_TABLES + ';' + databaseName);
            var serverResponse = tcpClient.ReadFromServer();

            foreach (var tableName in serverResponse.Split(';')[1].Split('|'))
            {
                int rowIndex = list_tables.Rows.Add();
                var row = list_tables.Rows[rowIndex];

                row.Cells[0].Value = tableName;

                if (selectedTables.Contains(tableName))
                {
                    row.Cells[1].Value = true;
                }
            }
        }

        public List<string> getSelectedTables()
        {
            return selectedTables;
        }
    }
}
