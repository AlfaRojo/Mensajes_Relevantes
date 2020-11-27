using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Laboratorio_3_EDII.Models;
using System;

namespace MRDB.Models
{
    public class Import
    {
        /// <summary>
        /// Importa el archivo a la carpeta UPLOAD para ser trabajado luego
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        private async Task<string> Import_FileAsync(IFormFile formFile)
        {
            var new_Path = string.Empty;
            var path = Path.Combine($"Upload", formFile.FileName);
            using (var this_file = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(this_file);
                new_Path = Path.GetFullPath(this_file.Name);
            }
            return new_Path;
        }

        public async Task<string> Upload_FileAsync(IFormFile formFile)
        {
            FileHandeling files = new FileHandeling();
            files.Create_File_Import();
            var new_path = await Import_FileAsync(formFile);
            files.Compress_LZW(new_path, Path.GetFileNameWithoutExtension(formFile.FileName));

            var full_Name = Path.GetFileNameWithoutExtension(formFile.FileName) + ".lzw";
            var path = Path.Combine($"Compress", full_Name);


            MongoHelper.ConnectToMongoService();
            MongoHelper.File_DB = MongoHelper.Database.GetCollection<FileDB>("Files");
            byte[] all_File = File.ReadAllBytes(path);
            var new_id = Guid.NewGuid().ToString();
            await MongoHelper.File_DB.InsertOneAsync(new FileDB
            {
                id = new_id,
                content = all_File,
                name = full_Name
            });
            files.Delete_Files_Compress();
            return new_id;
        }

    }
}
