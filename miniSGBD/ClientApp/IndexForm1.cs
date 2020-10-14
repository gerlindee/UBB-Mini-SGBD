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
    public partial class IndexForm1 : Form
    {
        private string databaseName;
        private string tablename;
        private Client tcpClient;

        private string uniqueIndex = "UNIQUE INDEX";
        private string nonUniqueIndex = "NONUNIQUE INDEX";

        public IndexForm1(Client cl, string db, string tb)
        {
            databaseName = db;
            tablename = tb;
            tcpClient = cl;
            InitializeComponent();

       
            IndexType.Items.Add(uniqueIndex);
            IndexType.Items.Add(nonUniqueIndex);
            IndexType.SelectedItem = uniqueIndex;

            populateFields();
        }


        private void populateFields()
        {
            attributeNames.Items.Clear();
            tcpClient.Write(Commands.CREATE_INDEX + ';' + databaseName + ';' + tablename);
            var serverResponse = tcpClient.ReadFromServer();
            var commandSplit = serverResponse.Split(';');

            try
            {
                var columnNames = commandSplit[1].Split('|');
                foreach (var tName in columnNames)
                    attributeNames.Items.Add(tName);
            }
            catch (Exception) { }

        }
        private void addIndex_Btn_Click(object sender, EventArgs e)
        {
            var checkedItems = attributeNames.CheckedItems;
            if (checkedItems.Count.Equals(0))
            {
                MessageBox.Show("At least one column must be selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var tableColumns = "";
            var indexName = "Index_" + tablename + '_';
            foreach (var item in checkedItems)
            {
                tableColumns += item.ToString() + '|';
                indexName += item.ToString() + '_';
            }

            tableColumns = tableColumns.Remove(tableColumns.Length - 1);
            indexName = indexName.Remove(indexName.Length - 1);
            if (IndexType.SelectedItem.ToString().Equals(uniqueIndex))
            {  
                tcpClient.Write(Commands.CREATE_UNIQUE_INDEX  + ';' + databaseName + ';' + tablename + ';' + tableColumns + ';' + indexName);
            }
            else
            {
                tcpClient.Write(Commands.CREATE_NONUNIQUE_INDEX + ';' + databaseName + ';' +tablename + ';' + tableColumns + ';' + indexName);
            }
            
            var serverResponse = tcpClient.ReadFromServer();
            MessageBox.Show(serverResponse, "Query Execution Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cancelIndex_btn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
