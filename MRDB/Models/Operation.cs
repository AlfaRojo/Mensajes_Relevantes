﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mensajes_Relevantes.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SDES;

namespace MRDB.Models
{
    public class Operation
    {
        public void CreateUser(string name, string nickName, string password)
        {
            Random rnd = new Random();
            MongoHelper.ConnectToMongoService();
            MongoHelper.User_Collection = MongoHelper.Database.GetCollection<User>("User");
            int DH_code = rnd.Next(250, 1022);
            password = Encrypt_Pass(password, DH_code);
            MongoHelper.User_Collection.InsertOneAsync(new User
            {
                Name = name,
                Nick_Name = nickName,
                Password = password,
                DH = DH_code
            });
        }

        private string Encrypt_Pass(string pass, int DH_code)
        {
            EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
            pass = encryptDecrypt.Encrypt(pass, Convert.ToString(DH_code, 2));
            return pass;
        }

        public bool SearchUser(string nickName, string password)
        {
            MongoHelper.ConnectToMongoService();
            var UserCollection = MongoHelper.Database.GetCollection<User>("User");
            var Users = UserCollection.AsQueryable<User>();

            var searchUser = from user in Users
                             where (string)user.Nick_Name == nickName && user.Password == password
                             select user;


            Data.Instance.user = searchUser.ToListAsync<User>().Result[0];
            EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
            var Result = (searchUser.Any<User>()) ? true : false;
            return Result;
        }

        public User Get_User_Info(string nickName)
        {
            MongoHelper.ConnectToMongoService();
            return MongoHelper.Database.GetCollection<User>("User").Find(d => d.Nick_Name == nickName).FirstOrDefault();
        }

        public void Insert_Chat(string text, string date, string emisor, string receptor, byte[] file_Cont, string fileName)
        {
            MongoHelper.ConnectToMongoService();
            MongoHelper.Message_Collection = MongoHelper.Database.GetCollection<Message>("Chat");
            MongoHelper.Message_Collection.InsertOneAsync(new Message
            {
                Text = text,
                SendDate = date,
                emisor = emisor,
                receptor = receptor,
                file_Content = file_Cont,
                file_Name = fileName
            });
        }

        public List<User> Get_Contacts(string user_name)
        {
            MongoHelper.ConnectToMongoService();
            return MongoHelper.Database.GetCollection<User>("User").Find(d => d.Nick_Name != user_name).ToListAsync().Result;
        }

        //Modifique
        public int Get_DH_Group(string emisor, string receptor)
        {
            MongoHelper.ConnectToMongoService();
            var UserCollection = MongoHelper.Database.GetCollection<User>("User");
            var User = UserCollection.Find(x => (string)x.Nick_Name == emisor).ToList();//Solo obtiene el primer amigo
            var Dh_Value = 0;
            int myFriend = 0;
            if (User.ElementAt(0) != null)
            {
                myFriend = User.ElementAt(0).Friends.Count;
                for (int i = 0; i < myFriend; i++)
                {
                    if (User.ElementAt(0).Friends.ElementAt(i).Nick_Name == receptor)
                    {
                        Dh_Value = User.ElementAt(0).Friends.ElementAt(i).DH_Key;
                    }
                }
            }
            return Dh_Value;
        }

        public List<Message> Get_Messages(string msg, string current_user)
        {
            List<Message> messages = new List<Message>();
            MongoHelper.ConnectToMongoService();
            var user_Info = MongoHelper.Database.GetCollection<User>("User").Find(d => d.Nick_Name == current_user).FirstOrDefault();
            if (user_Info != null)
            {
                var cant_Friend = user_Info.Friends.Count();
                for (int i = 0; i < cant_Friend; i++)
                {
                    var cypher_msg = get_Cypher_Message(user_Info.Friends[i].DH_Key, msg);
                    var message_Cyper_Emis = MongoHelper.Database.GetCollection<Message>("Chat").Find(d => d.emisor == current_user).ToListAsync();
                    if (message_Cyper_Emis.Result != null)
                    {
                        int count_msg = message_Cyper_Emis.Result.Count();
                        for (int j = 0; j < count_msg; j++)
                        {
                            if (message_Cyper_Emis.Result[j].Text == cypher_msg)
                            {
                                EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
                                var decrypted = encryptDecrypt.Decrypt(cypher_msg, Convert.ToString(user_Info.Friends[i].DH_Key, 2));
                                Message message = new Message
                                {
                                    Text = decrypted,
                                    emisor = message_Cyper_Emis.Result[j].emisor,
                                    receptor = message_Cyper_Emis.Result[j].receptor,
                                    SendDate = message_Cyper_Emis.Result[j].SendDate
                                };
                                messages.Add(message);
                            }
                        }
                    }
                    var message_Cyper_Recept = MongoHelper.Database.GetCollection<Message>("Chat").Find(d => d.receptor == current_user).ToListAsync();
                    if (message_Cyper_Recept.Result != null)
                    {
                        int count_msg = message_Cyper_Recept.Result.Count();
                        for (int j = 0; j < count_msg; j++)
                        {
                            if (message_Cyper_Recept.Result[j].Text == cypher_msg)
                            {
                                EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
                                var decrypted = encryptDecrypt.Decrypt(cypher_msg, Convert.ToString(user_Info.Friends[i].DH_Key, 2));
                                Message message = new Message
                                {
                                    Text = decrypted,
                                    emisor = message_Cyper_Recept.Result[j].emisor,
                                    receptor = message_Cyper_Recept.Result[j].receptor,
                                    SendDate = message_Cyper_Recept.Result[j].SendDate
                                };
                                messages.Add(message);
                            }
                        }
                    }
                }
                return messages;
            }
            return messages;
        }
        private string get_Cypher_Message(int DH, string msg)
        {
            EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
            var cypher = encryptDecrypt.Encrypt(msg, Convert.ToString(DH, 2));
            return cypher;
        }

        public List<Contact> Get_Friends(string current_user)
        {
            MongoHelper.ConnectToMongoService();
            var friends = MongoHelper.Database.GetCollection<User>("User").Find(d => d.Nick_Name == current_user).FirstOrDefault();
            return friends.Friends;
        }
    }
}
