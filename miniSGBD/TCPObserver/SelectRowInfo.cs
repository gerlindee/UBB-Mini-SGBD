using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class SelectRowInfo
    {
        public SelectRowInfo(string command)
        {
            // Select Information formatting: TableName#ColumnName#Aggregate#Alias#Output#Filter#Group#Having
            var columnStruct = command.Split('#');
            TableName = columnStruct[0];
            ColumnName = columnStruct[1];
            Alias = columnStruct[3];
            Aggregate = columnStruct[2];

            if(columnStruct[4] == SelectColumnInformation.Output)
            {
                Output = true;
            }

            Filter = columnStruct[5];

            if (columnStruct[6] == SelectColumnInformation.GroupBy)
            {
                GroupBy = true;
            }

            Having = columnStruct[7];
        }

        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string Aggregate { get; set; }
        public string Alias { get; set; }
        public bool Output { get; set; } = false;
        public bool GroupBy { get; set; } = false;
        public string Filter { get; set; }
        public string Having { get; set; } 
    }
}
