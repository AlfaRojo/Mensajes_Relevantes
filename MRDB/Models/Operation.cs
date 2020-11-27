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
        private string GenerateObjectId(string key)
        {
            var Key = (string)Convert.ChangeType(key, typeof(object));
            return Key;
        }

        public void CreateUser(string name, string nickName, string password)
        {
            Random rnd = new Random();
            MongoHelper.ConnectToMongoService();
            MongoHelper.User_Collection = MongoHelper.Database.GetCollection<User>("User");
            string id = GenerateObjectId(nickName);
            MongoHelper.User_Collection.InsertOneAsync(new User
            {
                Name = name,
                Nick_Name = id,
                Password = password,
                DH = rnd.Next(100, 800)
            });
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
            var UserCollection = MongoHelper.Database.GetCollection<User>("User");
            var Users = UserCollection.AsQueryable<User>();

            var searchUser = from user in Users
                             where (string)user.Nick_Name == nickName
                             select user;

            var DH_Value = searchUser.ToListAsync<User>().Result[0].DH;
            return DH_Value;
        }

        public FileDB Find_File(string id)
        {
            MongoHelper.ConnectToMongoService();
            var UserCollection = MongoHelper.Database.GetCollection<FileDB>("File");
            var filesDB = UserCollection.AsQueryable<FileDB>();

            var search_File = from myFile in filesDB
                             where (string)myFile.id == id
                             select myFile;

            var this_file = search_File.ToListAsync<FileDB>().Result[0];
            return this_file;
        }

        public void Insert_Chat(string text, string date, string fileID, string emisor)
        {
            var new_id = Guid.NewGuid().ToString();
            MongoHelper.ConnectToMongoService();
            MongoHelper.Message_Collection = MongoHelper.Database.GetCollection<Message>("Chat");
            MongoHelper.Message_Collection.InsertOneAsync(new Message { 
                Id_Message = new_id,
                Text = text,
                file_ID = fileID,
                SendDate = date,
                emisor = emisor
                
            });
        }

        public List<User> Get_Contacts(string user_name)
        {
            MongoHelper.ConnectToMongoService();
            return MongoHelper.Database.GetCollection<User>("User").Find(d => d.Nick_Name != user_name).ToListAsync().Result;
        }
    }
}
