using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ServerApp.Queries
{
    class SelectQuery 
    {
            private string DatabaseName;
            private string TableName;
            private MongoDBAcess MongoDB;

            public SelectQuery(string _databaseName, string _tableName)
            {
                DatabaseName = _databaseName;
                TableName = _tableName;
                MongoDB = new MongoDBAcess(DatabaseName);
            }

            public string Execute()
            {
                return SelectEntireTable();
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
