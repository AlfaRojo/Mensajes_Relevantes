using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Mensajes_Relevantes.Models
{
    public class Message
    {
        [BsonId]
        [BsonElement("idMessage")]
        public object Id_Message { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonElement("sendDate")]
        public DateTime SendDate { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("fileName")]
        public string FileName { get; set; }

        [BsonElement("contentFile")]
        public bool ContentFile { get; set; }

      }
}
