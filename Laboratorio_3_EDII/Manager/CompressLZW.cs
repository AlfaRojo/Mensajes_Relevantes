using System;
using System.Collections.Generic;
using System.IO;
using Laboratorio_3_EDII.Models;
using EDII_PROYECTO.Huffman;
using System.Linq;
using System.Text;

namespace Laboratorio_3_EDII.Manager
{
    public class CompressLZW 
    {
        public string Decompress_File(FileStream importFile, string _fileName)
        {
            var path = string.Empty;
            using (var reader = new BinaryReader(importFile))
            {
                var diccionaryCaracter = Convert.ToChar(reader.ReadByte());
                var countDiccionary = string.Empty;
                while (diccionaryCaracter != '.')
                {
                    countDiccionary += diccionaryCaracter;
                    diccionaryCaracter = Convert.ToChar(reader.ReadByte());
                }
                var countText = string.Empty;
                diccionaryCaracter = Convert.ToChar(reader.ReadByte());
                while (diccionaryCaracter != '.')
                {
                    countText += diccionaryCaracter;
                    diccionaryCaracter = Convert.ToChar(reader.ReadByte());
                }
                diccionaryCaracter = Convert.ToChar(reader.PeekChar());
                var writeByte = reader.ReadByte();
                var DiccionarioCar = new Dictionary<int, string>();
                while (DiccionarioCar.Count != Convert.ToInt32(countDiccionary))
                {
                    if (!DiccionarioCar.ContainsValue(Convert.ToString(Convert.ToChar(writeByte))))
                    {
                        DiccionarioCar.Add(DiccionarioCar.Count + 1, Convert.ToString(Convert.ToChar(writeByte)));
                    }
                    writeByte = reader.ReadByte();
                }
                reader.ReadByte();
                reader.ReadByte();
                diccionaryCaracter = Convert.ToChar(reader.ReadByte());
                var bitLenght = " ";
                while (diccionaryCaracter != '.')
                {
                    bitLenght += diccionaryCaracter;
                    diccionaryCaracter = Convert.ToChar(reader.ReadByte());
                }
                diccionaryCaracter = Convert.ToChar(reader.ReadByte());
                var extension = Path.GetExtension(importFile.Name);
                while (diccionaryCaracter != '\u0002')
                {
                    extension += diccionaryCaracter;
                    diccionaryCaracter = Convert.ToChar(reader.ReadByte());
                }
                extension = "." + extension;
                var byteNow = string.Empty;
                var compressList = new List<int>();
                reader.ReadByte();
                reader.ReadByte();
                while (reader.BaseStream.Position != reader.BaseStream.Length && compressList.Count < Convert.ToInt32(countText))
                {
                    var readedByte = Convert.ToString(reader.ReadByte(), 2);
                    while (readedByte.Length < 8)
                    {
                        readedByte = "0" + readedByte;
                    }
                    byteNow += readedByte;
                    if (Convert.ToInt32(bitLenght) > 8)
                    {
                        if (byteNow.Length >= Convert.ToInt32(bitLenght))
                        {
                            var thisComprimido = string.Empty;
                            for (int i = 0; i < Convert.ToInt32(bitLenght); i++)
                            {
                                thisComprimido += byteNow[i];
                            }
                            compressList.Add(Convert.ToInt32(thisComprimido, 2));
                            byteNow = byteNow.Substring(Convert.ToInt32(bitLenght));
                        }
                    }
                    else
                    {
                        compressList.Add(Convert.ToInt32(byteNow, 2));
                        byteNow = string.Empty;
                    }
                }
                if (byteNow.Length > 0)
                {
                    compressList[compressList.Count - 1] = compressList[compressList.Count - 1] + Convert.ToInt32(byteNow, 2);
                }
                var first = DiccionarioCar[compressList[0]];
                compressList.RemoveAt(0);
                var Decompressed = new System.Text.StringBuilder(first);
                importFile.Close();
                

                using (FileStream newFile = new FileStream($"Decompress/" + _fileName + ".txt", FileMode.OpenOrCreate))
                {
                    using (StreamWriter writer = new StreamWriter(newFile))
                    {
                        foreach (var item in compressList)
                        {
                            var analis = string.Empty;
                            if (DiccionarioCar.ContainsKey(item))
                            {
                                analis = DiccionarioCar[item];
                            }
                            else if (item == DiccionarioCar.Count + 1)
                            {
                                analis = first + first[0];
                            }
                            Decompressed.Append(analis);
                            DiccionarioCar.Add(DiccionarioCar.Count + 1, first + analis[0]);
                            first = analis;
                        }
                        writer.Write(Decompressed.ToString());
                    }
                }
            }
            path = $"Decompress/" + _fileName + ".txt";
            return path;
        }
        public void Compress_File(FileStream ArchivoImportado, string nameFile = null)
        {
            Dictionary<string, int> diccionario_LZW = new Dictionary<string, int>();
            var Extension = Path.GetExtension(ArchivoImportado.Name);
            var PropiedadesArchivoActual = new Files();
            FileInfo ArchivoAnalizado = new FileInfo(ArchivoImportado.Name);
            PropiedadesArchivoActual.TamanoArchivoDescomprimido = ArchivoAnalizado.Length;
            PropiedadesArchivoActual.NombreArchivoOriginal = ArchivoAnalizado.Name;
            var ListaCaracteresExistentes = new List<byte>();
            var ListaCaracteresBinario = new List<string>();
            var ASCIIescribir = new List<int>();
            using (var Lectura = new BinaryReader(ArchivoImportado))
            {
                using (FileStream writeStream = new FileStream($"Compress/" + nameFile + ".lzw", FileMode.OpenOrCreate))
                {
                    PropiedadesArchivoActual.RutaArchivoComprimido = Path.GetFullPath(writeStream.Name);
                    using (BinaryWriter writer = new BinaryWriter(writeStream))
                    {
                        const int BufferLength = 100;
                        var byteBuffer = new byte[BufferLength];
                        while (Lectura.BaseStream.Position != Lectura.BaseStream.Length)
                        {
                            byteBuffer = Lectura.ReadBytes(BufferLength);
                            foreach (var item in byteBuffer)
                            {
                                if (!ListaCaracteresExistentes.Contains(item))
                                {
                                    ListaCaracteresExistentes.Add(item);
                                }
                            }
                        }
                        ListaCaracteresExistentes.Sort();
                        foreach (var item in ListaCaracteresExistentes)
                        {
                            var caractreres = Convert.ToChar(item);
                            diccionario_LZW.Add(caractreres.ToString(), diccionario_LZW.Count + 1);
                        }
                        var diccionarioTam = Convert.ToString(diccionario_LZW.LongCount()) + ".";
                        writer.Write(diccionarioTam.ToCharArray());
                        Lectura.BaseStream.Position = 0;
                        var thisCaracter = string.Empty;
                        var myOutput = string.Empty;
                        while (Lectura.BaseStream.Position != Lectura.BaseStream.Length)
                        {
                            byteBuffer = Lectura.ReadBytes(BufferLength);
                            foreach (byte item in byteBuffer)
                            {
                                var toAnalizar = thisCaracter + Convert.ToChar(item);
                                if (diccionario_LZW.ContainsKey(toAnalizar))
                                {
                                    thisCaracter = toAnalizar;
                                }
                                else
                                {
                                    ASCIIescribir.Add(diccionario_LZW[thisCaracter]);
                                    diccionario_LZW.Add(toAnalizar, diccionario_LZW.Count + 1);
                                    thisCaracter = Convert.ToChar(item).ToString();
                                }
                            }
                        }
                        ASCIIescribir.Add(diccionario_LZW[thisCaracter]);
                        var textotamano = Convert.ToString(diccionario_LZW.LongCount()) + ".";
                        writer.Write(textotamano.ToCharArray());
                        foreach (var item in ListaCaracteresExistentes)
                        {
                            var Indice = Convert.ToByte(item);
                            writer.Write(Indice);
                        }
                        writer.Write(Environment.NewLine);
                        var mayorIndice = ASCIIescribir.Max();
                        var bitsIndiceMayor = (Convert.ToString(mayorIndice, 2)).Count();
                        writer.Write(bitsIndiceMayor.ToString().ToCharArray());
                        writer.Write(Extension.ToCharArray());
                        writer.Write(Environment.NewLine);
                        if (mayorIndice > 255)
                        {
                            foreach (var item in ASCIIescribir)
                            {
                                var indiceBinario = Convert.ToString(item, 2);
                                while (indiceBinario.Count() < bitsIndiceMayor)
                                {
                                    indiceBinario = "0" + indiceBinario;
                                }
                                ListaCaracteresBinario.Add(indiceBinario);
                            }
                            var allBits = string.Empty;
                            foreach (var item in ListaCaracteresBinario)
                            {
                                for (int i = 0; i < item.Length; i++)
                                {
                                    if (allBits.Count() < 8)
                                    {
                                        allBits += item[i];
                                    }
                                    else
                                    {
                                        var allDecimal = Convert.ToInt64(allBits, 2);
                                        var allBytes = Convert.ToByte(allDecimal);
                                        writer.Write((allBytes));
                                        allBits = string.Empty;
                                        allBits += item[i];
                                    }
                                }
                            }
                            if (allBits.Length > 0)
                            {
                                var allResultado = Convert.ToInt64(allBits, 2);
                                writer.Write(Convert.ToByte(allResultado));
                            }
                        }
                        else
                        {
                            foreach (var item in ASCIIescribir)
                            {
                                writer.Write(Convert.ToByte(Convert.ToInt32(item)));
                            }
                        }
                        List<Files> PilaArchivosComprimidos = new List<Files>();
                        PropiedadesArchivoActual.TamanoArchivoComprimido = writeStream.Length;
                        PropiedadesArchivoActual.FactorCompresion = Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoComprimido) / Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoDescomprimido);
                        PropiedadesArchivoActual.RazonCompresion = Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoDescomprimido) / Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoComprimido);
                        PropiedadesArchivoActual.PorcentajeReduccion = (Convert.ToDouble(1) - PropiedadesArchivoActual.FactorCompresion).ToString();
                        PilaArchivosComprimidos.Add(PropiedadesArchivoActual);
                        FileHandeling fileHandeling = new FileHandeling();
                        fileHandeling.Compressed(PilaArchivosComprimidos, Path.GetFileNameWithoutExtension(ArchivoImportado.Name), "LZW");
                    }
                }
            }
        }
        string OtroArchivo(string fileName, string sourcePath, string targetPath)
        {
            string sourceFile = Path.Combine(sourcePath, fileName);
            string destFile = Path.Combine(targetPath, fileName);
            {
                Directory.CreateDirectory(targetPath);
            }
            File.Copy(sourceFile, destFile, true);
            return targetPath;
        }

        //TamañoDiccionario.TamañoFinalDiccionario
        public string Compress_Text(string compress)
        {
            var TableLengthBegin = 0;
            var TableLengthFinal = 0;
            var Texto_Comprimido = string.Empty;
            List<string> CodeBinary = new List<string>();
            List<int> Code = new List<int>();
            Dictionary<int, string> CodeTable = new Dictionary<int, string>();
            foreach (char Character in compress)
            {
                if (!CodeTable.ContainsValue(Character.ToString()))
                {
                    CodeTable.Add(CodeTable.Count + 1, Character.ToString());
                }
            }
            TableLengthBegin = CodeTable.Count;
            Texto_Comprimido = TableLengthBegin + ".";

            var CharacterFirst = compress[0].ToString();
            var Text_Decimal = 0;
            for (int i = 1; i < compress.Length; i++)
            {
                var Concatanation = CharacterFirst.ToString() + compress[i];
                if (CodeTable.ContainsValue(Concatanation))
                {
                    if (i < compress.Length - 1) { CharacterFirst = Concatanation; }
                    else { Text_Decimal = CodeTable.FirstOrDefault(x => x.Value == Concatanation).Key; Code.Add(Text_Decimal); } 
                }
                else
                {
                    Text_Decimal = CodeTable.FirstOrDefault(x => x.Value == CharacterFirst).Key;
                    Code.Add(Text_Decimal);
                    CodeTable.Add(CodeTable.Count + 1, Concatanation);
                    CharacterFirst = compress[i].ToString();
                }
            }
            TableLengthFinal = CodeTable.Count;
            Texto_Comprimido += TableLengthFinal + ".";


            //addlongTableBegin.addlongTableFinal
            for (int i = 1; i <= TableLengthBegin; i++)
            {
                Texto_Comprimido += CodeTable.FirstOrDefault(x => x.Key == i).Value;
            }

            var MaxLengthInBinary = Convert.ToString(TableLengthFinal,2);
            foreach (var item in Code)
            {
                var CodeInBynary = Convert.ToString(item, 2);
                if (CodeInBynary.Length == MaxLengthInBinary.Length) { CodeBinary.Add(CodeInBynary); }
                else {  CodeInBynary = CodeInBynary.PadLeft(MaxLengthInBinary.Length, '0'); CodeBinary.Add(CodeInBynary); }
            }

            var TextBinary = string.Empty;
            foreach (var item in CodeBinary)
            {
                TextBinary += item;
            }

            if(TextBinary.Length % 8 != 0)
            {
                while(TextBinary.Length % 8 != 0)
                {
                    TextBinary += "0";
                }
            }
            var Data = GetBytesFromBinaryString(TextBinary);
            var TextASCII = Encoding.ASCII.GetString(Data);
            Texto_Comprimido += TextASCII;
            return Texto_Comprimido;

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
