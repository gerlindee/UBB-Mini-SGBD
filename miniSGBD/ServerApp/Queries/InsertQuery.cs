﻿using MongoDB.Bson;
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
        private MongoDBAcess MongoDB; 

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

        public override string ValidateQuery()
        {
            try
            {
                MongoDB = new MongoDBAcess(DatabaseName);
                return Responses.INSERT_INTO_TABLE_SUCCESS;

            } 
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public override void PerformXMLActions()
        {
            try
            {
                MongoDB.InsertKVIntoCollection("5", "test#test#test", TableName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
