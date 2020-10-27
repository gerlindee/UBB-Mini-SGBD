﻿using ClientApp;
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

            foreach (var columnInfo in retreivedInformation)
                columnInfoList.Add(new ColumnInfo(columnInfo));

            for (var i = 0; i < columnInfoList.Count; i++)
                dataGrid.Columns.Add(string.Format("col{0}", i), columnInfoList[i].ColumnName);
        }

        private void insertBtn_Click(object sender, EventArgs e)
        {
            var message =  Commands.INSERT_INTO_TABLE + ";" + databaseName + ";" + tablename + ";";
            var noRows = dataGrid.Rows.Count;
            
            if (dataGrid.Rows.Count > 1)
                noRows -= 1;

            for (int rows = 0; rows < noRows; rows++)
            {
                for (int col = 0; col < dataGrid.Rows[rows].Cells.Count; col++)
                {
                    if (validateCell(col, dataGrid.Rows[rows].Cells[col]))
                        message += dataGrid.Rows[rows].Cells[col].Value.ToString() + '*';
                    else
                        return;
                }
                message += '*';
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
            if (!Validator.isTypeCorrect(columnObject.Type, cell))
            {
                MessageBox.Show(Validator.WRONG_TYPE, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (columnObject.Lenght != -1 && Validator.isLenghtExceeded(columnObject.Lenght, cell.Length))
            {
                MessageBox.Show(Validator.EXCEEDED_LENGHT, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
