using Laboratorio_3_EDII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Mensajes_Relevantes.Models;

namespace MRDB.Models
{
    public class ManagerFile
    {
        public string FileAcction(string filename, byte[] fileCont)
        {
            Import import = new Import();
            var path = import.Import_BytesAsync(filename, fileCont);
            var filehandeling = new FileHandeling();
            filehandeling.Create_File_Export();
            var pathDecompress = filehandeling.Decompress_LZW(path, filename);
            return pathDecompress;
        }

        public byte[] GetFileConten(string receptor, string emisor, string fileName)
        {
            byte[] fileCont = new byte[1];
            MongoHelper.ConnectToMongoService();
            var HistoryCollection = MongoHelper.Database.GetCollection<History>("History");
            var User = HistoryCollection.AsQueryable<History>();
            var UserSearch = from user in User
                             where user.Emisor == emisor && user.Receptor == receptor
                             select user;

            var informationUser = UserSearch.ToList();
            var ListChat = new List<Message>();

            foreach (var item in informationUser)
            {
                foreach (var chat in item.Chat)
                {
                    if(chat.file_Name == fileName)
                    {   
                        fileCont = chat.file_Content;
                        break;
                    }
                }
            }
            return fileCont;
        }
    }
}
