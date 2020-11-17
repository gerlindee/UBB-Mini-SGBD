using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils;

namespace ServerApp.Queries
{
    class DeleteRowsQuery : AbstractQuery
    {
        private string DatabaseName;
        private string TableName;
        private string RemovedKey;
        private List<string> ForeignKeyFiles;

        public DeleteRowsQuery(string _databaseName, string _tableName, string _removedKey): base(Commands.DELETE_RECORD)
        {
            DatabaseName = _databaseName;
            TableName = _tableName;
            RemovedKey = _removedKey;
            ForeignKeyFiles = new List<string>();
        }

        public override void ParseAttributes()
        {
            // nothing to parse here :) 
        }

        public override string ValidateQuery()
        {
            if (TableUtils.IsTableReferenced(DatabaseName, TableName))
            {
                var MongoDB = new MongoDBAcess(DatabaseName);
                ForeignKeyFiles = TableUtils.GetForeignKeyFiles(DatabaseName, TableName);
                foreach (var foreignKey in ForeignKeyFiles)
                {
                    if (MongoDB.CollectionContainsKey(foreignKey, RemovedKey))
                    {
                        // PK of the record is used as a FK in another table => error
                        var tableName = foreignKey.Split('_');
                        return Responses.DELETE_RECORD_USED_AS_FK + tableName[1];
                    }
                }
            }

            return Responses.DELETE_RECORD_SUCCESS;
        } 

        public override void PerformXMLActions()
        {
            try
            {
                var mongoDB = new MongoDBAcess(DatabaseName);
                if (RemovedKey == "ALL")
                {
                    mongoDB.RemoveAllKVFromCollection(TableName);
                }
                else
                {
                    // Remove record PK from Unique Index File

                    // Remove record PK from FK value 
                    RemoveFromFKIndexFiles(mongoDB);

                    // Remove record from main table collection
                    mongoDB.RemoveKVFromCollection(TableName, RemovedKey);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void RemoveFromFKIndexFiles(MongoDBAcess mongoDB)
        {
            ForeignKeyFiles.Clear();
            ForeignKeyFiles = TableUtils.GetOwnForeignKeyFiles(DatabaseName, TableName);

            foreach (var foreignKey in ForeignKeyFiles)
            {
                mongoDB.RemoveValueFromCollection(foreignKey, RemovedKey);
            }
        }
    }
}
