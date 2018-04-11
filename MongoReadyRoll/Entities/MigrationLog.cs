using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoReadyRoll.Entities
{
    public class MigrationLog
    {
        [BsonId]
        public string Id { get; set; }
        public string ScriptFilename { get; set; }
        public DateTime ExecutionDate { get; set; }
    }
}
