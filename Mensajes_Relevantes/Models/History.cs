using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mensajes_Relevantes.Models
{
    public class History
    {
        [BsonId]
        public Object id_History { get; set; }

        [BsonElement("emisor")]
        public string Emisor { get; set; }

        [BsonElement("receptor")]
        public string Receptor { get; set; }

        [BsonElement("Chat")]
        public List<Message> Chat;

        public History()
        {
            Chat = new List<Message>();
        }

    }
}
