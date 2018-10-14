using System;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using MongoDbTest.Entities;
using MongoDB.Bson;

namespace MongoDbTest.Repositories
{
    internal class TestRepository
    {
        #region Properties

        private readonly MongoClient _client;

        #endregion

        public TestRepository()
        {
            _client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString);
        }

        public async Task<List<string>> GetDatabaseNames()
        {
            List<string> result = new List<string>();

            using (IAsyncCursor<BsonDocument> cursor = await _client.ListDatabasesAsync())
            {
                List<BsonDocument> databaseDocuments = await cursor.ToListAsync();
                foreach (BsonDocument databaseDocument in databaseDocuments)
                    result.Add(databaseDocument["name"].AsString);
            }

            return result;
        }

        public async Task<List<string>> GetCollectionsNames(string dbName)
        {
            List<string> result = new List<string>();

            IMongoDatabase database = _client.GetDatabase(dbName);

            using (IAsyncCursor<BsonDocument> collCursor = await database.ListCollectionsAsync())
            {
                List<BsonDocument> collections = await collCursor.ToListAsync();
                result.AddRange(collections.Select(c => c["name"].AsString));
            }

            return result;
        }

        public async void AddDocument(string dbName, string collectionName, Person person)
        {
            IMongoDatabase database = _client.GetDatabase(dbName);
            IMongoCollection<Person> collection = database.GetCollection<Person>(collectionName);
            await collection.InsertOneAsync(person);
        }

        public async Task<List<string>> FindDocs<T>(string dbName, string collectionName)
        {
            List<string> result = new List<string>();

            IMongoDatabase database = _client.GetDatabase(dbName);
            IMongoCollection<T> collection = database.GetCollection<T>(collectionName);
            BsonDocument filter = new BsonDocument();

            List<T> people = await collection.Find(filter).ToListAsync();
            foreach (T doc in people)
                result.Add(doc.ToBsonDocument().ToString());

            return result;
        }
    }
}
