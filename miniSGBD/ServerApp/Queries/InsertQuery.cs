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
            var mongoClient = new MongoClient("mongodb+srv://mongo_user:parolaMongo@cluster0.qsvie.mongodb.net/" + DatabaseName + "?retryWrites=true&w=majority");
            var mongoDatabase = mongoClient.GetDatabase(DatabaseName);
            var mongoCollection = mongoDatabase.GetCollection<BsonDocument>(TableName);
            BsonDocument newRecord = new BsonDocument().Add("_id", "2").Add("value", "test");
            mongoCollection.InsertOne(newRecord);
        }
    }
}
