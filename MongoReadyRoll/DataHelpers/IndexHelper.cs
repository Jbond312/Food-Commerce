using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace MongoReadyRoll.DataHelpers
{
    public class IndexHelper
    {
        public async Task Execute(IMongoDatabase db, JToken data, string collectionName)
        {
            var indexOptions = JsonConvert.DeserializeObject<IndexOptions>(data.ToString());

            var collection = db.GetCollection<BsonDocument>(collectionName);           

            if (indexOptions.Operation == IndexOperations.Drop)
            {
                await DroppingIndex(collection, indexOptions.IndexProperty.ToLower());
                return;
            }

            var indexes = await collection.Indexes.ListAsync();

            if (KeyExists(indexes, indexOptions.Name))
            {
                await DroppingIndex(collection, indexOptions.Name);
            }

            await collection.Indexes.CreateOneAsync(indexOptions.IndexKeyDefinition, new CreateIndexOptions
            {
                Name = indexOptions.Name,
                Unique = indexOptions.IsUnique
            });
        }

        private static bool KeyExists(IAsyncCursor<BsonDocument> indexes,
            string indexName)
        {
            while (indexes.MoveNext())
            {
                var currentBatchOfIndexes = indexes.Current;
                var allKeys = currentBatchOfIndexes.Select(x => x.Values);
                foreach (var key in allKeys)
                {
                    var existingIndexName = key.ToArray()[2].AsString;
                    if (existingIndexName == indexName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static async Task DroppingIndex(IMongoCollection<BsonDocument> collection, string name)
        {
            Console.WriteLine("Found and dropping Key: " + name);
            await collection.Indexes.DropOneAsync(name);
        }
    }
}