using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoReadyRoll.ExtensionMethods;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MongoReadyRoll.DataHelpers
{
    public class IndexOptions
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public IndexTypes IndexType { get; set; }
        public string IndexProperty { get; set; }
        [JsonProperty(PropertyName = "Unique")]
        public bool IsUnique { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Directions Direction { get; set; }
        public IndexOperations? Operation { get; set; }

        public string Name
        {
            get
            {
                var indexName = $"{IndexProperty}_{IndexType.Description().ToLower()}";

                if (IsUnique)
                {
                    indexName += "_Unique";
                }

                return indexName.ToLower();
            }
        }

        public IndexKeysDefinition<BsonDocument> IndexKeyDefinition
        {
            get
            {
                IndexKeysDefinition<BsonDocument> indexKey;
                switch (IndexType)
                {
                    case IndexTypes.SingleField:
                        if (Direction == Directions.Ascending)
                        {
                            indexKey = Builders<BsonDocument>.IndexKeys.Ascending(IndexProperty);
                        }
                        else
                        {
                            indexKey = Builders<BsonDocument>.IndexKeys.Descending(IndexProperty);
                        }
                        break;
                    case IndexTypes.TwoDSphere:
                        indexKey = Builders<BsonDocument>.IndexKeys.Geo2DSphere(IndexProperty);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return indexKey;
            }
        }
    }
}
