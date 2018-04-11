using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.Repository
{
    public class BaseEntity : IEntity
    {
        private string _id;

        [BsonId]
        public string Id
        {
            get => _id;
            set => _id = string.IsNullOrEmpty(value) ? ObjectId.GenerateNewId().ToString() : value;
        }
    }
}
