using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class SelectRowInfo
    {
        public SelectRowInfo(string command)
        {
            //colName#alias#output#filter#group#having
            var columnStruct = command.Split('#');
            ColumnName = columnStruct[0];
            Alias = columnStruct[1];
            Filter = columnStruct[3];
            Having = columnStruct[5];

            if(columnStruct[2] == SelectColumnInformation.Output)
            {
                Output = true;
            }

            if (columnStruct[4] == SelectColumnInformation.GroupBy)
            {
                GroupBy = true;
            }
        }

        public string ColumnName { get; set; }
        public string Alias { get; set; }
        public bool Output { get; set; } = false;
        public bool GroupBy { get; set; } = false;
        public string Filter { get; set; }
        public string Having { get; set; } 
    }
}
