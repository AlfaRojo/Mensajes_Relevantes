using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DiffieHelman;
using SDES;

namespace MRDB.Models
{
    public class Charecter
    {
        public int Decimal { get; set; }

        public string Character { get; set; }

        public void GetCharacter()
        {
            MongoHelper.ConnectToMongoService();
            MongoHelper.Character_Collection = MongoHelper.Database.GetCollection<Charecter>("Character");
            for (int i = 0; i < 256; i++)
            {
                Charecter charecter = new Charecter();
                charecter.Decimal = i;
                charecter.Character = Convert.ToString(Convert.ToChar(i));
                MongoHelper.Character_Collection.InsertOneAsync(new Charecter
                {
                    Decimal = charecter.Decimal,
                    Character = charecter.Character
                }) ;
            }
        }

        public void SerchCharacter(string character)
        {

            MongoHelper.ConnectToMongoService();
            var CharacterCollection = MongoHelper.Database.GetCollection<Charecter>("Character");
            var Character = CharacterCollection.AsQueryable<Charecter>();

            var searchUser = from charecter in Character
                             where charecter.Character == ","
                             select charecter;

            foreach (var item in searchUser)
            {
                var p = item.Character;
            }



            var a = 0; 
        }


    }
}
