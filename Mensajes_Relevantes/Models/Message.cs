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

      }
}
