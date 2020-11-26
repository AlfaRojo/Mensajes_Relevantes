using System;
using Mensajes_Relevantes.Models;
using MongoDB.Driver;

namespace MRDB.Models
{
    public class MongoHelper
    {
        public static IMongoClient Client { get; set; }
        public static IMongoDatabase Database { get; set; }

        public static string MongoConnection = "mongodb+srv://EbGue:7vkLmhPq9X3@newclouster.rbdey.mongodb.net/test?authSource=admin&replicaSet=atlas-rbpzt0-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true";
        public static string MongoDataBase = "MRDB";


        public static IMongoCollection<User> User_Collection { get; set; }
        public static IMongoCollection<Chat> Chat_Collection { get; set; }
        public static IMongoCollection<History> History_Collection { get; set; }
        public static IMongoCollection<Message> Message_Collection { get; set; }

        internal static void ConnectToMongoService()
        {
            try
            {
                Client = new MongoClient(MongoConnection);
                Database = Client.GetDatabase(MongoDataBase);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
