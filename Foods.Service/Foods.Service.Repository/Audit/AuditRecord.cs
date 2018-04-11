using System;
using System.Collections.Generic;
using Common.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Foods.Service.Repository.Audit
{
    public class AuditRecord : BaseEntity
    {
        private DateTime _createdOn;
        public string AuthToken { get; set; }
        public string HttpVerb { get; set; }
        public string QueryString { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public List<AuditLog> Logs { get; set; }

        [BsonRepresentation(BsonType.Document)]
        public DateTime CreatedOn
        {
            get => _createdOn;
            set => _createdOn = value == DateTime.MinValue ? DateTime.UtcNow : value;
        }
    }
}
