using MongoDB.Bson;
using MongoDB.Driver;
using System;
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
        private string[] Records;

        public InsertQuery(string _databaseName, string _tableName, string _records) : base(Commands.INSERT_INTO_TABLE)
        {
            DatabaseName = _databaseName;
            TableName = _tableName;
            RecordsString = _records;
        }

        public override void ParseAttributes()
        {
            Records = RecordsString.Split('|');
        }

        public override void PerformXMLActions()
        {
            try
            {
                var mongoDB = new MongoDBAcess(DatabaseName);
                mongoDB.InsertKVIntoCollection("5", "test#test#test", TableName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
