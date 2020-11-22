using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mensajes_Relevantes.Models
{
    public class Chat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("idChat")]
        public string id_Chat { get; set; }


        public List<Message> SendMessages;
        public List<Message> ReciveMessages;

        public Chat()
        {
            SendMessages = new List<Message>();
            ReciveMessages = new List<Message>();
        }
    }
}
