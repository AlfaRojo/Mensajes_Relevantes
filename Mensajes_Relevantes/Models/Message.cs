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
        public string EmisorMsg { get; set; }
        public string ReceptorMsg { get; set; }
        public string Mensaje { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime FechaEnvio { get; set; }
        public bool PoseeArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public byte[] Archivo { get; set; }
    }
}
