using Mensajes_Relevantes.Models;
using MRDB.IService;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using DiffieHelman;
using System;

namespace MRDB.Models
{
    public class UserInformation : IUser
    {
        public List<Contact> UserContacts;
        public List<User> _User;
        public List<History> _History;
        public List<Contact> GetAllContacts()
        {
            MongoHelper.ConnectToMongoService();
            var ContactCollection = MongoHelper.Database.GetCollection<Contact>("Contact");
            UserContacts = ContactCollection.Find(d => true).ToList();
            return UserContacts;
        }

        public List<User> GetAllUser()
        {
            MongoHelper.ConnectToMongoService();
            var UserCollection = MongoHelper.Database.GetCollection<User>("User");
            _User = UserCollection.Find(d => true).ToList();
            return _User;
        }

        //Modificado
        public List<History> GetAllHistory()
        {
            MongoHelper.ConnectToMongoService();
            var HistoryCollection = MongoHelper.Database.GetCollection<History>("History");
            _History = HistoryCollection.Find(x => true).ToList();
            return _History;
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
                if (!Search)
                {
                    MongoHelper.Contact_Collection.InsertOneAsync(new Contact
                    {
                        Nick_Name = (string)item.Nick_Name
                    });

                }
            }
        }

        //Modificado
        public void Add_Contact(Contact new_Contact, string ActualUser)
        {
            GetAllUser();
            MongoHelper.ConnectToMongoService();
            var User1 = _User.Find(d => d.Nick_Name == ActualUser);
            var User2 = _User.Find(d => d.Nick_Name == new_Contact.Nick_Name);
            var user = User1;
            if (user.Friends.Count > 0)
            {
                var Search = User1.Friends.Exists(x => x.Nick_Name == new_Contact.Nick_Name);
                if (!Search)
                {
                    new_Contact.DH_Key = Convert.ToInt32(DiffieH.DiffieHelmannAlgorithm(User1.DH, User2.DH).ToString());
                    user.Friends.Add(new_Contact);
                    MongoHelper.Database.GetCollection<User>("User").FindOneAndReplace(x => (string)x.Nick_Name == ActualUser, user);
                }
            }
            else
            {
                new_Contact.DH_Key = Convert.ToInt32(DiffieH.DiffieHelmannAlgorithm(User1.DH, User2.DH).ToString());
                user.Friends.Add(new_Contact);
                MongoHelper.Database.GetCollection<User>("User").FindOneAndReplace(x => (string)x.Nick_Name == ActualUser, user);
            }
            Contact contact = new Contact
            {
                Nick_Name = User1.Nick_Name,
                DH_Key = User1.DH
            };
            other_Contact(contact, User2.Nick_Name);
        }
        //Modificado
        private void other_Contact(Contact new_Contact, string ActualUser)
        {
            MongoHelper.ConnectToMongoService();
            var User1 = _User.Find(d => d.Nick_Name == ActualUser);
            var User2 = _User.Find(d => d.Nick_Name == new_Contact.Nick_Name);
            var user = User1;
            if (user.Friends.Count > 0)
            {
                var Search = User1.Friends.Exists(x => x.Nick_Name == new_Contact.Nick_Name);
                if (!Search)
                {
                    new_Contact.DH_Key = User2.Friends[0].DH_Key;
                    user.Friends.Add(new_Contact);
                    MongoHelper.Database.GetCollection<User>("User").FindOneAndReplace(x => (string)x.Nick_Name == ActualUser, user);
                }
            }
            else
            {
                new_Contact.DH_Key = User2.Friends[0].DH_Key;
                user.Friends.Add(new_Contact);
                MongoHelper.Database.GetCollection<User>("User").FindOneAndReplace(x => (string)x.Nick_Name == ActualUser, user);
            }
        }

        //Modficado
        public void SetHistoryCollection(string emisor, string receptor, Message message)
        {
            MongoHelper.ConnectToMongoService();
            MongoHelper.History_Collection = MongoHelper.Database.GetCollection<History>("History");
            var Conversation1 = MongoHelper.History_Collection.AsQueryable<History>();
            var SearchC1 = from history in Conversation1
                           where history.Emisor == emisor && history.Receptor == receptor
                           select history;

            var SearhC2 = from history in Conversation1
                          where history.Emisor == receptor && history.Receptor == emisor
                          select history;

            if (!SearchC1.Any())
            {

                var History = new History();
                History.id_History = new string($"{emisor}{receptor}");
                History.Emisor = emisor;
                History.Receptor = receptor;
                History.Sent.Add(message);
                MongoHelper.History_Collection.InsertOneAsync(History);
                if (!SearhC2.Any())
                {
                    var History2 = new History();
                    History2.id_History = new string($"{receptor}{emisor}");
                    History2.Emisor = receptor;
                    History2.Receptor = emisor;
                    History2.Received.Add(message);
                    MongoHelper.History_Collection.InsertOneAsync(History2);
                }
            }
            else
            {
                GetAllHistory();
                var UserConversation1 = _History.Find(x => x.Emisor == emisor && x.Receptor == receptor);
                var UserConversation2 = _History.Find(x => x.Emisor == receptor && x.Receptor == emisor);
                UserConversation1.Sent.Add(message);
                MongoHelper.History_Collection.FindOneAndReplace(x => x.Emisor == emisor && x.Receptor == receptor, UserConversation1);
                UserConversation2.Received.Add(message);
                MongoHelper.History_Collection.FindOneAndReplace(x => x.Emisor == receptor && x.Receptor == emisor, UserConversation2);

            }

        }

        //Modificado
        public UserInformation()
        {
            UserContacts = new List<Contact>();
            _User = new List<User>();
            _History = new List<History>();
        }
    }
}
