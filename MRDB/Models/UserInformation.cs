using Mensajes_Relevantes.Models;
using MRDB.IService;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using DiffieHelman;

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
            var user = new User();
            MongoHelper.ConnectToMongoService();
            var UserCollection = MongoHelper.Database.GetCollection<User>("User");
            var User1 = _User.Find(x => (string)x.Nick_Name == ActualUser);
            var User2 = _User.Find(x => (string)x.Nick_Name == id.Nick_Name);
            user = User1;

            if(user.Friends.Count > 0 )
            {
                var Search = User1.Friends.Exists(x => x.id_Contact == id.id_Contact);
                if (!Search)
                {
                    user.Get_DH();
                    id.Key = DiffieH.DiffieHelmannAlgorithm(User1.DH, User2.DH).ToString();
                    user.Friends.Add(id);
                    UserCollection.FindOneAndReplace(x => (string)x.Nick_Name == ActualUser, user);
                   
                }
            }
            else
            {
                user.Get_DH();
                id.Key = DiffieH.DiffieHelmannAlgorithm(User1.DH, User2.DH).ToString();
                user.Friends.Add(id);
                UserCollection.FindOneAndReplace(x => (string)x.Nick_Name == ActualUser, user);
            }            
        }
        public UserInformation()
        {
            UserContacts = new List<Contact>();
            _User = new List<User>();
        }
    }
}
