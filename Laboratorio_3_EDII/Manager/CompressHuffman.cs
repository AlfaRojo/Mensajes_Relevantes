using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using EDII_PROYECTO.Huffman;
using Laboratorio_3_EDII.Helper;
using Laboratorio_3_EDII.IService;
using System.Threading.Tasks;
using Laboratorio_3_EDII.Models;

namespace Laboratorio_3_EDII.Manager
{
    public class CompressHuffman : ICompression
    {
        /// <summary>
        /// Obtiene un archivo de texto para devolver un archivo comprimido con extensión .huff
        /// </summary>
        /// <param name="fileToCompress"></param>
        /// <param name="name"></param>
        public void Compress_File(FileStream fileToCompress, string name)
        {
            var huffman = new Huffman();
            var PropiedadesArchivoActual = new Files();
            PropiedadesArchivoActual.TamanoArchivoDescomprimido = fileToCompress.Length;
            var full_path = $"Compress\\" + name + ".huff";
            fileToCompress.Close();
            var direccion = Path.GetFullPath(fileToCompress.Name);
            int cantidadCaracteres = huffman.Read(direccion);
            huffman.Create_Tree();
            byte[] encabezado = huffman.Create_Header(cantidadCaracteres);
            using (FileStream ArchivoComprimir = new FileStream(full_path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                PropiedadesArchivoActual.NombreArchivoOriginal = Path.GetFileNameWithoutExtension(fileToCompress.Name);
                PropiedadesArchivoActual.RutaArchivoComprimido = Path.GetFullPath(ArchivoComprimir.Name);
                foreach (var item in encabezado)
                {
                    ArchivoComprimir.WriteByte(item);
                }
                int bufferLength = 80;
                var buffer = new byte[bufferLength];
                string textoCifrado = string.Empty;
                using (var file = new FileStream(fileToCompress.Name, FileMode.Open))
                {
                    using (var reader = new BinaryReader(file))
                    {
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            buffer = reader.ReadBytes(bufferLength);
                            foreach (var item in buffer)
                            {
                                int posiList;
                                posiList = Data.Instance.codeList.FindIndex(x => x.caracter == item);
                                textoCifrado = textoCifrado + Data.Instance.codeList.ElementAt(posiList).codigo;
                                if ((textoCifrado.Length / 8) > 0)
                                {
                                    string escribirByte = textoCifrado.Substring(0, 8);
                                    byte byteEscribir = Convert.ToByte(escribirByte, 2);
                                    ArchivoComprimir.WriteByte(byteEscribir);
                                    textoCifrado = textoCifrado.Substring(8);
                                }
                            }
                        }
                        reader.ReadBytes(bufferLength);
                        List<Files> PilaArchivosComprimidos = new List<Files>();
                        PropiedadesArchivoActual.TamanoArchivoComprimido = ArchivoComprimir.Length;
                        PropiedadesArchivoActual.RazonCompresion = Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoComprimido) / Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoDescomprimido);
                        PropiedadesArchivoActual.FactorCompresion = Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoDescomprimido) / Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoComprimido);
                        PropiedadesArchivoActual.PorcentajeReduccion = (Convert.ToDouble(1) - PropiedadesArchivoActual.RazonCompresion).ToString();
                        PilaArchivosComprimidos.Add(PropiedadesArchivoActual);
                        FileHandeling fileHandeling = new FileHandeling();
                        fileHandeling.Compressed(PilaArchivosComprimidos, Path.GetFileNameWithoutExtension(fileToCompress.Name), "Huffman");
                    }
                }
                if (textoCifrado.Length > 0 && (textoCifrado.Length % 8) == 0)
                {
                    byte byteEsc = Convert.ToByte(textoCifrado, 2);
                }
                else if (textoCifrado.Length > 0)
                {
                    textoCifrado = textoCifrado.PadRight(8, '0');
                    byte byteEsc = Convert.ToByte(textoCifrado, 2);
                }
            }
        }

        /// <summary>
        /// Obtiene un archivo comprimido con extensión huff y devuelve el archivo original con extensión .txt
        /// </summary>
        /// <param name="fileToDecompress"></param>
        public void Decompress_File(FileStream fileToDecompress)
        {
            FileHandeling fileHandeling = new FileHandeling();
            var fileName = fileHandeling.Get_Name("Huffman", fileToDecompress.Name);
            var full_path = $"Decompress\\" + fileName + ".txt";
            using (FileStream archivo = new FileStream(full_path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                int contador = 0;
                int contadorCarac = 0;
                int CantCaracteres = 0;
                int CaracteresDif = 0;
                string texto = string.Empty;
                string acumula = "";
                byte auxiliar = 0;
                int bufferLength = 80;
                var buffer = new byte[bufferLength];
                string textoCifrado = string.Empty;
                fileToDecompress.Close();
                using (var file = new FileStream(fileToDecompress.Name, FileMode.Open))
                {
                    using (var reader = new BinaryReader(file))
                    {
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            buffer = reader.ReadBytes(bufferLength);
                            foreach (var item in buffer)
                            {

                                if (contador == ((CaracteresDif * 2) + 2) && contadorCarac < CantCaracteres)
                                {
                                    texto = Convert.ToString(item, 2);
                                    if (texto.Length < 8)
                                    {
                                        texto = texto.PadLeft(8, '0');
                                    }
                                    acumula = acumula + texto;
                                    int cont = 0;
                                    int canteliminar = 0;
                                    string validacion = "";
                                    foreach (var item2 in acumula)
                                    {
                                        validacion = validacion + item2;
                                        cont++;
                                        if (Data.Instance.DicCarcacteres.ContainsKey(validacion))
                                        {
                                            archivo.WriteByte(Data.Instance.DicCarcacteres[validacion]);
                                            acumula = acumula.Substring(cont);
                                            cont = 0;
                                            contadorCarac++;
                                            canteliminar = cont;
                                            validacion = "";
                                        }
                                    }
                                }
                                if (item != 44)
                                {
                                    byte[] byteCarac = { item };
                                    texto = texto + Encoding.ASCII.GetString(byteCarac);
                                }
                                if (item == 44 && contador > 1 && contador < ((CaracteresDif * 2) + 2))
                                {
                                    if (item == 44 && contador % 2 == 0)
                                    {
                                        auxiliar = Convert.ToByte(texto, 2);
                                        texto = string.Empty;
                                        contador++;
                                    }
                                    else if (contador % 2 != 0 && item == 44)
                                    {
                                        Data.Instance.DicCarcacteres.Add(texto, auxiliar);
                                        texto = string.Empty;
                                        contador++;
                                    }
                                }
                                else
                                {
                                    if (item == 44 && contador == 0)
                                    {
                                        CantCaracteres = int.Parse(texto);
                                        texto = string.Empty;
                                        contador++;
                                    }
                                    else if (item == 44 && contador == 1)
                                    {
                                        CaracteresDif = int.Parse(texto);
                                        texto = string.Empty;
                                        contador++;
                                    }
                                }
                            }
                        }
                        reader.ReadBytes(bufferLength);
                    }
                }
            };
            Data.Instance.DicCarcacteres.Clear();
        }

        public string CompressionHuffman(string Cadena)
        {
            string TextoCifrado = string.Empty;
            var CantidadCaracteres = 0;
            var Huffman = new Huffman();
            CantidadCaracteres = Huffman.Read_Str(Cadena);
            var ListadoCodigos = Huffman.CrearTree();
          
            for (int i = 0; i < Cadena.Length; i++)
            {
                foreach (var caracterList in ListadoCodigos)
                {
                    byte caracter = Convert.ToByte(Cadena[i]);
                    if (caracter == caracterList.caracter)
                    {
                        TextoCifrado += caracterList.codigo;
                    }
                }
            }
            var txtCompreso = string.Empty;
            if (TextoCifrado.Length % 8 != 0)
            {
                while (TextoCifrado.Length % 8 != 0)
                {
                    TextoCifrado += "0";
                }
            }
            var data = GetBytesFromBinaryString(TextoCifrado);
            var txt = Encoding.ASCII.GetString(data);
            return txt;
        }

        public Byte[] GetBytesFromBinaryString(string binary)
        {
            var list = new List<Byte>();

            for (int i = 0; i < binary.Length; i += 8)
            {
                string t = binary.Substring(i, 8);

                list.Add(Convert.ToByte(t, 2));
            }

            return list.ToArray();
        }
    }
}
