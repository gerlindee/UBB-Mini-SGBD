using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ServerApp.Queries
{
    class SelectQuery : AbstractQuery
    {
        private string DatabaseName;
        private string TableName;
        private string Attributes;
        private MongoDBAcess MongoDB;

        private List<SelectRowInfo> SelectRows = new List<SelectRowInfo>();
        private List<Tuple<string,string>> OutputParamsAndAlias = new List<Tuple<string, string>>();
        private List<Tuple<string, string>> FilterParamsAndCondition = new List<Tuple<string, string>>();
        private List<string> GroupByList = new List<string>();

        public SelectQuery(string _databaseName, string _tableName, string _attributes) : base(Commands.SELECT_RECORDS)
        {
            DatabaseName = _databaseName;
            TableName = _tableName;
            Attributes = _attributes;
            MongoDB = new MongoDBAcess(DatabaseName);
        }

        public override void ParseAttributes()
        {
            var rows = new List<SelectRowInfo>();
            var splitAttributes = Attributes.Split('|');

            foreach(var attribute in splitAttributes)
            {
                var selectRowInfo = new SelectRowInfo(attribute);
                SelectRows.Add(selectRowInfo);
            }
        }

        public void parseRows()
        {
            foreach(var row in SelectRows)
            {
                if (row.Alias != "-")
                    OutputParamsAndAlias.Add(new Tuple<string, string>(row.ColumnName, row.Alias));
                if (row.Filter != "-")
                    FilterParamsAndCondition.Add(new Tuple<string, string>(row.ColumnName, row.Filter));
                if (row.GroupBy)
                    GroupByList.Add(row.ColumnName);

            }
        }
        private string SelectEntireTable()
        {
            try
            {
                var records = "";
                var keyValuePairs = MongoDB.GetEntireCollection(TableName);
                foreach (var keyValue in keyValuePairs)
                {
                    records += keyValue.GetElement("_id").Value + "#" + keyValue.GetElement("value").Value + "|";
                }
                return Commands.MapCommandToSuccessResponse(Commands.SELECT_RECORDS) + ";" + records;
            }
            catch (Exception ex)
            {
                return ex.Message + ";";
            }
        }

    }
}
