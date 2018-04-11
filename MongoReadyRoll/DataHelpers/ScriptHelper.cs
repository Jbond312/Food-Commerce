using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace MongoReadyRoll.DataHelpers
{
    public static class ScriptHelper
    {
        public static async Task Execute(IMongoDatabase db, JToken data, string collectionName, string keyPropertyName)
        {
            const string bsonId = "_id";
            var collection = db.GetCollection<BsonDocument>(collectionName);

            foreach (dynamic record in data.Children())
            {
                string keyPropertyValue = record[keyPropertyName];
                var builder = Builders<BsonDocument>.Filter;
                var filter = builder.Eq(keyPropertyName, keyPropertyValue);
                var existingRecord = await collection.Find(filter).FirstOrDefaultAsync();

                var id = existingRecord == null
                    ? ObjectId.GenerateNewId().ToString()
                    : existingRecord.GetValue(bsonId).ToString();
                record._id = id;

                var json = record.ToString();
                var bson = BsonDocument.Parse(json);
                var filterDefinition = builder.Eq(bsonId, id);

                await collection.ReplaceOneAsync(filterDefinition, bson,
                    new UpdateOptions
                    {
                        IsUpsert = true
                    });
            }
        }
    }
}