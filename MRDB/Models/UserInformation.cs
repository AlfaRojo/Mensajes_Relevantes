﻿using Mensajes_Relevantes.Models;
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

        public void Add_Contact(Contact new_Contact, string ActualUser)
        {
            MongoHelper.ConnectToMongoService();
            var User1 = MongoHelper.Database.GetCollection<User>("User").Find(d => d.Nick_Name == ActualUser).ToListAsync().Result[0];
            var User2 = MongoHelper.Database.GetCollection<User>("User").Find(d => d.Nick_Name == new_Contact.Nick_Name).ToListAsync().Result[0];
            var user = User1;
            if (user.Friends.Count > 0)
            {
                user.Get_DH();
                new_Contact.DH_Key = Convert.ToInt32(DiffieH.DiffieHelmannAlgorithm(User1.DH, User2.DH).ToString());
                user.Friends.Add(new_Contact);
                MongoHelper.Database.GetCollection<User>("User").FindOneAndReplace(x => (string)x.Nick_Name == ActualUser, user);
            }
            else
            {
                user.Get_DH();
                new_Contact.DH_Key = Convert.ToInt32(DiffieH.DiffieHelmannAlgorithm(User1.DH, User2.DH).ToString());
                user.Friends.Add(new_Contact);
                MongoHelper.Database.GetCollection<User>("User").FindOneAndReplace(x => (string)x.Nick_Name == ActualUser, user);
            }
        }
        public UserInformation()
        {
            UserContacts = new List<Contact>();
            _User = new List<User>();
        }
    }
}
