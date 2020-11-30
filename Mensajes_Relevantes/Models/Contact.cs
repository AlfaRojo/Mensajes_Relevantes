using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Mensajes_Relevantes.Models
{
    public class Contact 
    {

        [BsonElement("Nick_Name")]
        public string Nick_Name { get; set; }

        [BsonElement("DH_Key")]
        public int DH_Key { get; set; }
    }
}
