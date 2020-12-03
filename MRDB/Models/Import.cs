using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Laboratorio_3_EDII.Models;

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

        public async Task<byte[]> Upload_FileAsync(IFormFile formFile)
        {
            FileHandeling files = new FileHandeling();
            files.Create_File_Import();
            var new_path = await Import_FileAsync(formFile);
            files.Compress_LZW(new_path, Path.GetFileNameWithoutExtension(formFile.FileName));

            var full_Name = Path.GetFileNameWithoutExtension(formFile.FileName) + ".lzw";
            var path = Path.Combine($"Compress", full_Name);

            byte[] all_File = File.ReadAllBytes(path);
            files.Delete_Files_Compress();
            return all_File;
        }

        //Prueba
        public string Import_BytesAsync(string _FileName, byte[] fileConten)
        {
            var new_Path = string.Empty;
            var fileHandelig = new FileHandeling();
            fileHandelig.Create_File_Import();
            var path = Path.Combine($"Upload", _FileName);
            new_Path = $"{path}.lzw";
            save_File(new_Path, fileConten);
            return new_Path;
        }
        private void save_File(string new_Path, byte[] txtResultado)
        {
            using (var writeStream = new FileStream(new_Path, FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    writer.Write(txtResultado);
                }
            }
        }
    }
}
