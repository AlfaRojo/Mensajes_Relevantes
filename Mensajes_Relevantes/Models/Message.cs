using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mensajes_Relevantes.Models
{
    public class Message
    {
        [BsonId]
        [BsonElement("idMessage")]
        public object Id_Message { get; set; }
        
        [BsonElement("sendDate")]
        public string SendDate { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("fileName")]
        public IFormFile FileName { get; set; }

        [BsonElement("file_ID")]
        public string file_ID { get; set; }

        [BsonElement("emisor")]
        public string emisor { get; set; }

        [BsonElement("receptor")]
        public string receptor { get; set; }

    }
}
