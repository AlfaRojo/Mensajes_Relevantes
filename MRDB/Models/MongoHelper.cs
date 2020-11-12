using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MRDB.Models
{
    public class MongoHelper
    {
        public static IMongoClient Client { get; set; }
        public static IMongoDatabase Database { get; set; }
        
        public static string MongoConnection = "mongodb://127.0.0.1:27017";

        public static string MongoDataBase = "MRDB";

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
