using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Queries
{
    class DeleteRowsQuery : AbstractQuery
    {
        private string DatabaseName;
        private string TableName;
        private string RemovedKey;

        public DeleteRowsQuery(string _databaseName, string _tableName, string _deleteFlag, string _removedKey)
        {
            DatabaseName = _databaseName;
            TableName = _tableName;
            RemovedKey = _removedKey;
        }

        public override void ParseAttributes()
        {
            // nothing to parse here :) 
        }

        public override void PerformXMLActions()
        {
            try
            {
                var mongoDB = new MongoDBAcess(DatabaseName);
                if (RemovedKey == "ALL")
                {

                }
                else
                {
                    mongoDB.RemoveKVFromCollection(TableName, RemovedKey);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
