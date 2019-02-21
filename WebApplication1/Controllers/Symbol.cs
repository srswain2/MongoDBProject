using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication1.Controllers
{
    [BsonIgnoreExtraElements]
    public class Symbol
    {
        public string Name { get; set; }
        [BsonId]
        public ObjectId _id { get; set; }

        public int Value { get; set; }
    }
}