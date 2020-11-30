using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mensajes_Relevantes.Models
{
    public class History
    {
        public Object id_History { get; set; }

        [BsonElement("emisor")]
        public string Emisor { get; set; }

        [BsonElement("receptor")]
        public string Receptor { get; set; }

        [BsonElement("Sent")]
        public List<Message> Sent;

        [BsonElement("Recived")]
        public List<Message> Received;

        public History()
        {
            Sent = new List<Message>();
            Received = new List<Message>();
        }

    }
}
