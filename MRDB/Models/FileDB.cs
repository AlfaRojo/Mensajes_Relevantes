using MongoDB.Bson.Serialization.Attributes;

namespace MRDB.Models
{
    public class FileDB
    {
        [BsonId]
        [BsonElement("_id")]
        public object id { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("content")]
        public byte[] content { get; set; }
    }
}
