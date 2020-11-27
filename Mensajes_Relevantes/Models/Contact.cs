﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Mensajes_Relevantes.Models
{
    public class Contact 
    {
        [BsonId]
        [BsonElement("id_Contact")]
        public string id_Contact { get; set; }

        [BsonElement("Nick_Name")]
        public string Nick_Name { get; set; }

        [BsonElement("Key")]
        public string Key { get; set; }
    }
}
