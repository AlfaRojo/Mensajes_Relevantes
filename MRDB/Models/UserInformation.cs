using Mensajes_Relevantes.Models;
using MRDB.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace MRDB.Models
{
    public class UserInformation : IUser
    {
        public List<Contact> UserContacts;
        public List<User> _User;
        public List<Contact> GetAllContacts()
        {
            MongoHelper.ConnectToMongoService();
            var ContactCollection = MongoHelper.Database.GetCollection<Contact>("Contact");
            UserContacts = ContactCollection.Find(d => true).ToList();
            return UserContacts;
        }

        public List<User> GetAllUser()
        {
            return _User;
        }

        public void SetContactCollection()
        {
            MongoHelper.ConnectToMongoService();
            var UserCollection = MongoHelper.Database.GetCollection<User>("User");
            MongoHelper.Contact_Collection = MongoHelper.Database.GetCollection<Contact>("Contact");
            _User = UserCollection.Find(d => true).ToList();
            foreach (var item in _User)
            {
                var Search = MongoHelper.Contact_Collection.Find(x => x.Nick_Name == (string)item.Nick_Name).Any();
                if(!Search)
                {
                     MongoHelper.Contact_Collection.InsertOneAsync(new Contact
                     {

                            Nick_Name = (string)item.Nick_Name
                     });

                }
            }
        }

        public void SetContactUser(Contact id, string ActualUser)
        {
            MongoHelper.ConnectToMongoService();
            var UserCollection = MongoHelper.Database.GetCollection<User>("User");
            foreach (var item in _User)
            {
                var Search = _User.Find(x => x.Friends.Exists(F => F.Nick_Name == id.Nick_Name));
                
            }
        }
        public UserInformation()
        {
            UserContacts = new List<Contact>();
            _User = new List<User>();
        }
    }
}
