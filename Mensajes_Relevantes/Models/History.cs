using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mensajes_Relevantes.Models
{
    public class History
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("idContact")]
        public string Id_Contact { get; set; }
        
        [BsonElement("emisor")]
        public string Emisor { get; set; }
        
        [BsonElement("receptor")]
        public string Receptor { get; set; }
        
        [BsonElement("idChat")]
        public string Id_Chat { get; set; }
    }
}
