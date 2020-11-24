using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Mensajes_Relevantes.Models
{
    [Serializable]
    public class User
    {
        [BsonId]
        [BsonElement("nickName")]
        public Object Nick_Name { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("HD")]
        public int DH { get; set; }

        public List<Contact> Friends;

        public User()
        {
            Friends = new List<Contact>();
        }

        public User(object nickname, string name, string password, int DH, List<Contact> friends)
        {
            this.Nick_Name = nickname;
            this.Name = name;
            this.Password = password;
            this.DH = DH;
            this.Friends = friends;
        }
    }
}
