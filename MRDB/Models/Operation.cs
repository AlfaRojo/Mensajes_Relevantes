using System;
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
            var ORIGINAL = encryptDecrypt.Decrypt(Data.Instance.user.Password, "0110100101");
            var Result = (searchUser.Any<User>()) ? true : false;
            return Result;
        }

        public int Find_DH(string nickName)
        {
            MongoHelper.ConnectToMongoService();
            return MongoHelper.Database.GetCollection<User>("User").Find(d => d.Nick_Name == nickName).ToListAsync().Result[0].DH; ;
        }

        public void Insert_Chat(string text, string date, string emisor, byte[] file_Cont, string fileName)
        {
            var new_id = Guid.NewGuid().ToString();
            MongoHelper.ConnectToMongoService();
            MongoHelper.Message_Collection = MongoHelper.Database.GetCollection<Message>("Chat");
            MongoHelper.Message_Collection.InsertOneAsync(new Message
            {
                Id_Message = new_id,
                Text = text,
                SendDate = date,
                emisor = emisor,
                file_Content = file_Cont,
                file_Name = fileName
            });
        }

        public List<User> Get_Contacts(string user_name)
        {
            MongoHelper.ConnectToMongoService();
            return MongoHelper.Database.GetCollection<User>("User").Find(d => d.Nick_Name != user_name).ToListAsync().Result;
        }

        public int Get_DH_Group(string emisor, string receptor)
        {
            MongoHelper.ConnectToMongoService();
            return MongoHelper.Database.GetCollection<User>("User").Find(d => d.Name == emisor).ToListAsync().Result[0].Friends[0].DH_Key;
        }

        public Message Get_Individual(string msg)
        {
            MongoHelper.ConnectToMongoService();
            var res = MongoHelper.Database.GetCollection<Message>("Chat").Find(d => d.Text == msg).ToListAsync().Result[0];
            Message message = new Message
            {
                Text = res.Text,
                emisor = res.emisor,
                receptor = res.receptor
            };
            return message;
        }
    }
}
