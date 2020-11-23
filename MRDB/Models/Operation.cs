using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mensajes_Relevantes.Models;
using MongoDB.Bson;
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

        public void CreateUser(string name,  string nickName, string password)
        {
            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.User_Collection = Models.MongoHelper.Database.GetCollection<User>("User");
            var Operation = new Operation();
            Object id = Operation.GenerateObjectId(nickName);
            Models.MongoHelper.User_Collection.InsertOneAsync(new User
            {
                Name = name,
                Nick_Name = id,
                Password = password
            });

        }

        public bool SearchUser(string nickName, string password)
        {
            Models.MongoHelper.ConnectToMongoService();
            var UserCollection = MongoHelper.Database.GetCollection<User>("User");
            var Users = UserCollection.AsQueryable<User>();

            var searchUser = from user in Users
                             where (string)user.Nick_Name == nickName && user.Password == password
                             select user;

           

            var Result = (searchUser.Any<User>()) ? true : false;
            return Result;
           
        }
    }
}
