using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    class MongoDBAcess
    {
        private string DatabaseName;
        private IMongoDatabase MongoDatabase;

        public MongoDBAcess(string _databaseName)
        {
            DatabaseName = _databaseName;
            CreateDBConnection();
        }

        private void CreateDBConnection()
        {
            try
            {
                var mongoClient = new MongoClient("mongodb+srv://mongo_user:parolaMongo@cluster0.qsvie.mongodb.net/" + DatabaseName + "?retryWrites=true&w=majority");
                MongoDatabase = mongoClient.GetDatabase(DatabaseName);
            }
            catch (Exception)
            {
                throw new Exception("Could not create MongoDB connection");
            }
        }

        public void InsertKVIntoCollection(string collectionName, string key, string value)
        {
            try
            {
                var mongoCollection = MongoDatabase.GetCollection<BsonDocument>(collectionName);
                BsonDocument newRecord = new BsonDocument().Add("_id", key).Add("value", value);
                mongoCollection.InsertOne(newRecord);
            }
            catch (Exception)
            {
                throw new Exception("Could not add Key-Value pair to MongoDB Cluster");
            }
        }

        public List<BsonDocument> GetEntireCollection(string collectionName)
        {
            try
            {
                var mongoCollection = MongoDatabase.GetCollection<BsonDocument>(collectionName);
                return mongoCollection.Find(new BsonDocument()).ToList();
            }
            catch (Exception)
            {
                throw new Exception("Could not retrieve the contents of MongoDB Collection: " + collectionName);
            }
        }
    }
}
