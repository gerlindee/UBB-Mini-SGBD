using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    class SelectColumnInfo
    {
        public SelectColumnInfo(string tableName, string command)
        {
            var selectConfig = command.Split('#');

            TableName = tableName;
            ColumnName = selectConfig[0];
            Alias = selectConfig[1];
            Output = bool.Parse(selectConfig[2]);
            Filter = new List<string>();
            foreach (var condition in selectConfig[3].Split('*'))
            {
                Filter.Add(condition);
            }
            Sorting = selectConfig[4];
            Having = selectConfig[5];
        }

        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string Alias { get; set; }
        public bool Output { get; set; }
        public List<string> Filter { get; set; }
        public string Sorting { get; set; }
        public string Having { get; set; }
    }
}
