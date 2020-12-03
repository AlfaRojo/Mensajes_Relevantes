using Mensajes_Relevantes.Models;
using MRDB.IService;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using DiffieHelman;
using System;
using SDES;

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
        public void SetHistoryCollection(Message message)
        {
            MongoHelper.ConnectToMongoService();
            MongoHelper.History_Collection = MongoHelper.Database.GetCollection<History>("History");
            var Conversation1 = MongoHelper.History_Collection.AsQueryable<History>();
            var SearchC1 = from history in Conversation1
                           where history.Emisor == message.emisor && history.Receptor == message.receptor
                           select history;

            var SearhC2 = from history in Conversation1
                          where history.Emisor == message.receptor && history.Receptor == message.emisor
                          select history;

            var message1 = new Message()
            {
                Id_Message = message.Id_Message,
                SendDate = message.SendDate,
                Text = message.Text,
                file_Name = message.file_Name,
                file_Content = message.file_Content,
                emisor = message.emisor,
                receptor = message.receptor,
                Action = "Send"
            };
            var message2 = new Message()
            {
                Id_Message = message.Id_Message,
                SendDate = message.SendDate,
                Text = message.Text,
                file_Name = message.file_Name,
                file_Content = message.file_Content,
                emisor = message.emisor,
                receptor = message.receptor,
                Action = "Recieved"
            };

            if (!SearchC1.Any())
            {
                var History = new History();
                History.id_History = new string($"{message.emisor}{message.receptor}");
                History.Emisor = message.emisor;
                History.Receptor = message.receptor;
                History.Chat.Add(message1);
                MongoHelper.History_Collection.InsertOneAsync(History);
                if (!SearhC2.Any())
                {
                    var History2 = new History();
                    History2.id_History = new string($"{message.receptor}{message.emisor}");
                    History2.Emisor = message.receptor;
                    History2.Receptor = message.emisor;
                    History2.Chat.Add(message2);
                    MongoHelper.History_Collection.InsertOneAsync(History2);
                }
            }
            else
            {
                GetAllHistory();
                var UserConversation1 = _History.Find(x => x.Emisor == message.emisor && x.Receptor == message.receptor);
                var UserConversation2 = _History.Find(x => x.Emisor == message.receptor && x.Receptor == message.emisor);
                UserConversation1.Chat.Add(message1);
                MongoHelper.History_Collection.FindOneAndReplace(x => x.Emisor == message.emisor && x.Receptor == message.receptor, UserConversation1);
                UserConversation2.Chat.Add(message2);
                MongoHelper.History_Collection.FindOneAndReplace(x => x.Emisor == message.receptor && x.Receptor == message.emisor, UserConversation2);

            }

        }

        //Modificado
        public List<Message> GetHistoryCollection(string emisor, string receptor)
        {
            var _ListChatCipher = new List<Message>();
            var _ListChatDesCipher = new List<Message>();
            var _History = new List<History>();
            MongoHelper.ConnectToMongoService();
            MongoHelper.History_Collection = MongoHelper.Database.GetCollection<History>("History");
            var Conversation1 = MongoHelper.History_Collection.AsQueryable<History>();
            var UserHisory = from history in Conversation1
                           where history.Emisor == emisor && history.Receptor == receptor
                           select history;

            //Decifrar el mensaje
            _History = UserHisory.ToList();
            Operation operation = new Operation();
            var DH_Group = 0;
            EncryptDecrypt encryptDecrypt = new EncryptDecrypt();

            foreach (var item in _History)
            {
                _ListChatCipher = item.Chat;
            }

            foreach(var item in _ListChatCipher)
            {
                if(item.Text != null)
                {
                    if (item.Action == "Recieved") { DH_Group = operation.Get_DH_Group(receptor, emisor); }
                    else if (item.Action == "Send") { DH_Group = operation.Get_DH_Group(emisor, receptor); }

                    var TextDesCipher = encryptDecrypt.Decrypt(item.Text, Convert.ToString(DH_Group, 2));
                    item.Text = TextDesCipher; 
                }
                _ListChatDesCipher.Add(item);
            }
            return _ListChatDesCipher;
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
