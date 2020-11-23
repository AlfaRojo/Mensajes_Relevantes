using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Mensajes_Relevantes.Models
{
    public class UserProperty
    {
        
        [BsonId]
        public Object Id { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
