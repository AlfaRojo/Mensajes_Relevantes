﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Mensajes_Relevantes.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;



namespace MRDB.Models
{
    public class Operation
    {
        public object GenerateObjectId(string key)
        {
            var Key = (object)Convert.ChangeType(key, typeof(object));
            return Key;
        }

        public void CreateUser(string name, string nickName, string password)
        {
            Random rnd = new Random();
            MongoHelper.ConnectToMongoService();
            MongoHelper.User_Collection = MongoHelper.Database.GetCollection<User>("User");
            var Operation = new Operation();
            Object id = Operation.GenerateObjectId(nickName);
            MongoHelper.User_Collection.InsertOneAsync(new User
            {
                Name = name,
                Nick_Name = id,
                Password = password,
                DH = rnd.Next(15, 200)
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
    }
}
