using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Foods.Service.Repository.Audit
{
    public class AuditLog
    {
        private DateTime _createdOn;
        public string Data { get; set; }
        [BsonRepresentation(BsonType.Document)]
        public DateTime CreatedOn
        {
            get => _createdOn;
            set => _createdOn = value == DateTime.MinValue ? DateTime.UtcNow : value;
        }
    }
}