using EDII_PROYECTO.Huffman;
using Laboratorio_3_EDII.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Laboratorio_3_EDII.Models
{
    public class FileHandeling
    {

        /// <summary>
        /// Create Upload and Compress files
        /// </summary>
        public void Create_File_Import()
        {
            if (!Directory.Exists($"Upload"))
            {
                Directory.CreateDirectory($"Upload");
            }
            if (!Directory.Exists($"Compress"))
            {
                Directory.CreateDirectory($"Compress");
            }
        }

        /// <summary>
        /// Create Upload and Decompress files
        /// </summary>
        public void Create_File_Export()
        {
            if (!Directory.Exists($"Download"))
            {
                Directory.CreateDirectory($"Download");
            }
            if (!Directory.Exists($"Decompress"))
            {
                Directory.CreateDirectory($"Decompress");
            }
        }

        /// <summary>
        /// LZW compress files
        /// </summary>
        /// <param name="new_Path"></param>
        /// <param name="name"></param>
        public void Compress_LZW(string new_Path, string name)
        {
            using (var new_File = new FileStream(new_Path, FileMode.Open))
            {
                CompressLZW compressLZW = new CompressLZW();
                compressLZW.Compress_File(new_File, name);
            }
            Delete_Files_Upload();
        }

        /// <summary>
        /// LZW decompress files
        /// </summary>
        /// <param name="new_Path"></param>
        /// <param name="name"></param>
        public void Decompress_LZW(string new_Path)
        {
            using (var new_File = new FileStream(new_Path, FileMode.Open))
            {
                CompressLZW compressLZW = new CompressLZW();
                compressLZW.Decompress_File(new_File);
            }
            Delete_Files_Upload();
        }
        private void Delete_Files_Upload()
        {
            DirectoryInfo di = new DirectoryInfo(@"Upload");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            if (Directory.Exists(@"Upload"))
            {
                Directory.Delete(@"Upload");
            }
        }
        public void Delete_Files_Compress()
        {
            DirectoryInfo di = new DirectoryInfo(@"Compress");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            if (Directory.Exists(@"Compress"))
            {
                Directory.Delete(@"Compress");
            }
        }

        public string Get_Name(string type, string compressName)
        {
            var full_path = $"Compress\\Factores de Compresion " + type + ".txt";
            var fileName = string.Empty;
            using (var nameFile = new StreamReader(full_path))
            {
                var line = string.Empty;
                while ((line = nameFile.ReadLine()) != null)
                {
                    fileName = line;
                    var ruta = nameFile.ReadLine();
                    var fullname = Path.GetFileName(compressName);
                    if (!(ruta.EndsWith(fullname)))//Encontrar el nombre del archivo en la ruta
                    {
                        for (int i = 0; i < 4; i++)//Saltar a las siguientes lineas del archivo
                        {
                            nameFile.ReadLine();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return fileName;
        }
        public void Compressed(List<Files> List_file, string name, string type)
        {
            var full_path = $"Compress\\Factores de Compresion " + type + ".txt";
            using (StreamWriter writer = File.AppendText(full_path))
            {
                foreach (var item in List_file)
                {
                    writer.WriteLine(item.NombreArchivoOriginal);
                    writer.WriteLine(item.RutaArchivoComprimido);
                    writer.WriteLine(item.RazonCompresion);
                    writer.WriteLine(item.FactorCompresion);
                    writer.WriteLine(item.PorcentajeReduccion);
                    writer.WriteLine(type);
                }
            }
        }
    }
}
