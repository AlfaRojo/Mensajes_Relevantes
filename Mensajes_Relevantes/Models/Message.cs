using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Mensajes_Relevantes.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string issuingMsg { get; set; }
        public string recipientMsg { get; set; }
        public string msg { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime sendDate { get; set; }
        public bool contentFile { get; set; }
        public string fileName { get; set; }
        public byte[] chatFile { get; set; }
    }
}
