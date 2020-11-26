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
        public IEnumerable<Contact> GetAllContacts()
        {
            MongoHelper.ConnectToMongoService();
            var ContactCollection = MongoHelper.Database.GetCollection<Contact>("Contact");
            UserContacts = ContactCollection.Find(d => true).ToList();
            return UserContacts;
        }

        public User GetUser()
        {
            throw new NotImplementedException();
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

        public UserInformation()
        {
            UserContacts = new List<Contact>();
            _User = new List<User>();
        }
    }
}
