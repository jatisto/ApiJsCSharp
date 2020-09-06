using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.DomainContext.Models;
using api.DomainContext.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.DomainContext.MongoDB
{
    public class MongoDbCRUD
    {
        private readonly IMongoDatabase _db;

        public MongoDbCRUD(MongoSettingsDb settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _db = client.GetDatabase(settings.Database);
        }

        public IMongoCollection<BsonDocument> GetCollection(string table)
        {
            return _db.GetCollection<BsonDocument>(table);
        }

        public void Insert<T>(string table, T insert)
        {
            var collection = _db.GetCollection<T>(table);
            collection.InsertOne(insert);
        }
        
        public void Update<T>(string table, string field,  List<T> value, ObjectId id)
        {
            var collection = _db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", id);
            var update = Builders<T>.Update.Set(field: field, value: value);
            collection.UpdateOneAsync(filter, update);
        }

        public T GetFilter<T>(string table, ObjectId id)
        {
            var collection = _db.GetCollection<T>(table);
            var filterDefinition = Builders<T>.Filter.Eq("_id", id);
            return collection.Find(filterDefinition).FirstOrDefault();
        }
        
        public List<BsonDocument> GetFilterElemMatch(string table, string nameArrayField, string nameKey, string nameValue)
        {
            var collection = _db.GetCollection<BsonDocument>(table);
            var highExamScoreFilter = Builders<BsonDocument>.Filter.ElemMatch<BsonValue>(nameArrayField, new BsonDocument { { nameKey, nameValue }});
            return collection.Find(highExamScoreFilter).ToList();
        }
        
        public List<BsonDocument> GetAll(string table)
        {
            var collection = GetCollection(table);
            return collection.Find(new BsonDocument()).ToList();
        }

        public void Delete<T>(string table, ObjectId id)
        {
            var collection = _db.GetCollection<T>(table);
            var filterDefinition = Builders<T>.Filter.Eq("_id", id);
            collection.DeleteOne(filterDefinition);
        }

        public async Task<T> FindDocs<T>(string table)
        {
            var collection = _db.GetCollection<T>(table);
            var filter = new BsonDocument();
            var people = await collection.Find(filter).ToListAsync();
            T result = default(T);
            foreach (T doc in people)
            {
                result = doc;
            }

            return result;
        }
        
    }
}