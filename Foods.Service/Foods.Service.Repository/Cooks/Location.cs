using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Foods.Service.Repository.Cooks
{
    public class Location
    {
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("coordinates")]
        public IEnumerable<double> Coordinates { get; set; }
    }
}
