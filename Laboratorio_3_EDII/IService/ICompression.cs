using System;
using System.IO;
namespace Laboratorio_3_EDII.IService
{
    public interface ICompression
    {
        public void Compress_File(FileStream Archivo, string name = null);
        public void Decompress_File(FileStream ArchivoImportado);
    }
}
