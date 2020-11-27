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
        [BsonElement("Nick_Name")]
        public string Nick_Name { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("DH")]
        public int DH { get; set; }

        public List<Contact> Friends;

        public User()
        {
            Friends = new List<Contact>();
            
        }

        public User(string nickname, string name, string password, int DH, List<Contact> friends)
        {
            this.Nick_Name = nickname;
            this.Name = name;
            this.Password = password;
            this.DH = DH;
            this.Friends = friends;
        }
        public void Set_Nick(string user)
        {
            this.Nick_Name = user;
        }
        public void Set_Name(string name)
        {
            this.Name = name;
        }
        public void Set_Diffie(int DH)
        {
            this.DH = DH;
        }
        public void Set_Friends(List<Contact> friends)
        {
            this.Friends = friends;
        }
        public string Get_Nick()
        {
            return (string)Nick_Name;
        }
        public string Get_Name()
        {
            return Name;
        }
        public int Get_DH()
        {
            return DH;
        }
        public List<Contact> Get_Contacts()
        {
            return Friends;
        }
    }
}
