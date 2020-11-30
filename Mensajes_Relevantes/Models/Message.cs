using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mensajes_Relevantes.Models
{
    public class Message
    {
        [BsonId]
        [BsonElement("idMessage")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id_Message { get; set; }

        [BsonElement("sendDate")]
        public string SendDate { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }


        [BsonElement("file_Name")]
        public string file_Name { get; set; }

        [BsonElement("file_Content")]
        public byte[] file_Content { get; set; }

        [BsonElement("emisor")]
        public string emisor { get; set; }

        [BsonElement("receptor")]
        public string receptor { get; set; }

    }
}
