using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ServerApp.Queries
{
    class InsertQuery : AbstractQuery
    {
        private string DatabaseName;
        private string TableName;
        private string RecordsString;
        private List<KeyValuePair<String, String>> Records;

        public InsertQuery(string _databaseName, string _tableName, string _records) : base(Commands.INSERT_INTO_TABLE)
        {
            DatabaseName = _databaseName;
            TableName = _tableName;
            RecordsString = _records;
            Records = new List<KeyValuePair<String, String>>();
        }

        public override void ParseAttributes()
        {
            var recordsArray = RecordsString.Split('|');
            foreach (var newRecord in recordsArray)
            {
                var keyValuePair = newRecord.Split('*');

                // Special handling for tables with one column, where key = value bc I said so 
                if (keyValuePair.Length == 1)
                {
                    Records.Add(new KeyValuePair<string, string>(keyValuePair[0], keyValuePair[0]));
                } 
                else
                {
                    Records.Add(new KeyValuePair<string, string>(keyValuePair[0], keyValuePair[1]));
                }
            }
        }

        public override void PerformXMLActions()
        {
            try
            {
                var mongoDB = new MongoDBAcess(DatabaseName);
                foreach (var keyValuePairs in Records)
                {
                    mongoDB.InsertKVIntoCollection(TableName, keyValuePairs.Key, keyValuePairs.Value);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("A record with Primary Key " + ex.Message.Split(':')[1] + " already exists in table " + TableName + "!" );
            }
        }
    }
}
