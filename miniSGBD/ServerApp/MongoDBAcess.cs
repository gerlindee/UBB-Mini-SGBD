using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    class MongoDBAcess
    {
        private string DatabaseName;
        private string CollectionName;
        private IMongoCollection<BsonDocument> MongoCollection; 

        public MongoDBAcess(string _databaseName, string _collectionName)
        {
            DatabaseName = _databaseName;
            CollectionName = _collectionName;
            CreateDBConnection();
        }

        private void CreateDBConnection()
        {
            try
            {
                var mongoClient = new MongoClient("mongodb+srv://mongo_user:parolaMongo@cluster0.qsvie.mongodb.net/" + DatabaseName + "?retryWrites=true&w=majority");
                MongoCollection = mongoClient.GetDatabase(DatabaseName).GetCollection<BsonDocument>(CollectionName);
            }
            catch (Exception)
            {
                throw new Exception("Could not create MongoDB connection");
            }
        }

        public void InsertKVIntoCollection(string key, string value)
        {
            try
            {
                BsonDocument newRecord = new BsonDocument().Add("_id", key).Add("value", value);
                MongoCollection.InsertOne(newRecord);
            }
            catch (Exception)
            {
                throw new Exception("Could not add Key-Value pair to MongoDB cluster");
            }
        }
    }
}
