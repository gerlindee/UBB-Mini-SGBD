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
    public partial class StatementsResultForm : Form
    {
        private List<string> tableHeader;
        private List<string> tableContent; 

        public StatementsResultForm(List<string> _header, List<string> _content)
        {
            tableHeader = _header;
            tableContent = _content; 

            InitializeComponent();

            setupTableColumns();
            setupTableContent();
        }

        private void setupTableColumns()
        {
            for (var i = 0; i < tableHeader.Count; i++)
            {
                table_contents_list.Columns.Add(string.Format("col{0}", i), tableHeader[i]);
            }
        }

        private void setupTableContent()
        {
            foreach (string tableRecord in tableContent)
            {
                if (tableRecord != "")
                {
                    var tableRecordSplit = tableRecord.Split('#');

                    int rowIndex = table_contents_list.Rows.Add();
                    var row = table_contents_list.Rows[rowIndex];

                    // Special handling for tables with one column 
                    if (tableHeader.Count == 1)
                    {
                        row.Cells[0].Value = tableRecordSplit[0];
                    }
                    else
                    {
                        for (int idx = 0; idx < tableRecordSplit.Length; idx++)
                        {
                            row.Cells[idx].Value = tableRecordSplit[idx];
                        }
                    }
                }
            }
        }
    }
}
