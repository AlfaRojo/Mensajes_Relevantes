using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Mensajes_Relevantes.Models
{
    public class Contact 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id_Contact { get; set; }
        public string Nick_Name { get; set; }
        //public System.Numerics.BigInteger Key { get; set; }
    }
}
