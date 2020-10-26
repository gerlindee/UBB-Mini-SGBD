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
    public partial class InsertForm : Form
    {
        private string databaseName;
        private string tablename;
        private Client tcpClient;
        private List<ColumnInfo> columnInfoList = new List<ColumnInfo>();

        public InsertForm(Client cl, string db, string tb)
        {
            databaseName = db;
            tablename = tb;
            tcpClient = cl;
            InitializeComponent();
            this.Text = tablename;

            setupTableStructure();
        }

        private void setupTableStructure()
        {
            tcpClient.Write(Commands.GET_TABLE_COLUMNS + ";" + databaseName + ";" + tablename);
            var serverResponse = tcpClient.ReadFromServer().Split(';');
            var retreivedInformation = serverResponse[1].Split('|');

            parseColumn(retreivedInformation);

            var colNames = new List<string>();
            for (var i = 0; i < columnInfoList.Count; i++)
                dataGrid.Columns.Add(string.Format("col{0}", i), columnInfoList[i].ColumnName);
        }

        private void parseColumn(string[] splitColumns)
        {
            foreach (var columnInfo in splitColumns) 
            { 
                var newCol = new ColumnInfo();
                var columnStruct = columnInfo.Split('#');
                newCol.ColumnName = columnStruct[0];

                for(var i=1; i< columnStruct.Length; i++)
                {
                    switch (columnStruct[i])
                    {
                        case ColumnInformation.PK:
                            {
                                newCol.PK = true;
                            }
                            break;
                        case ColumnInformation.FK:
                            {
                                newCol.FK = true;
                            }
                            break;
                        case ColumnInformation.UNQ:
                            {
                                newCol.unique = true;
                            }
                            break;
                        case ColumnInformation.NULL:
                            {
                                newCol.nonNull = true;
                            }
                            break;
                        default:
                            {
                                try
                                {
                                    var types = columnStruct[i].Split('-');
                                    newCol.type = types[0];
                                    int.TryParse(types[1], out int val);
                                    newCol.lenght = val;
                                }
                                catch (Exception)
                                {
                                    newCol.type = columnStruct[i];
                                }
                            }
                            break;
                    }
                }
                columnInfoList.Add(newCol);
            }
        }

        private void insertBtn_Click(object sender, EventArgs e)
        {
            var message = "";// Commands. + ";" + databaseName + "#" + text_table_name.Text + "#";
            var noRows = dataGrid.Rows.Count;
            if (dataGrid.Rows.Count > 1)
                noRows -= 1;

            for (int rows = 0; rows < noRows; rows++)
            {
                for (int col = 0; col < dataGrid.Rows[rows].Cells.Count; col++)
                {
                    if (validateCell(col, dataGrid.Rows[rows].Cells[col]))
                        message += dataGrid.Rows[rows].Cells[col].Value.ToString() + ';';
                    else
                        return;
                }
                message += '|';
                message = message.Remove(message.Length - 2);
            }

            tcpClient.Write(message);
            var serverResponse = tcpClient.ReadFromServer();
            if (serverResponse == Commands.MapCommandToSuccessResponse(Commands.CREATE_TABLE))
            {
                MessageBox.Show(serverResponse, "Query Execution Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                MessageBox.Show(serverResponse, "Query Execution Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool validateCell(int columnIndex, DataGridViewCell gridCell)
        {
            if (gridCell.Value == null)
            {
                MessageBox.Show(Validator.EMPTY_FIELD, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var cell = gridCell.Value.ToString();
            //get column object 
            var columnName = dataGrid.Columns[columnIndex].HeaderText.ToString();
            var columnObject = columnInfoList.Find(c => c.ColumnName == columnName);

            if (Validator.isStringEmpty(cell))
            {
                MessageBox.Show(Validator.EMPTY_FIELD, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!Validator.isTypeCorrect(columnObject.type, cell))
            {
                MessageBox.Show(Validator.WRONG_TYPE, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (columnObject.lenght != -1 && Validator.isLenghtExceeded(columnObject.lenght, cell.Length))
            {
                MessageBox.Show(Validator.EXCEEDED_LENGHT, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
