using System;
using System.IO;
using System.Collections.Generic;
using Laboratorio_3_EDII.Models;
using Laboratorio_3_EDII.IService;
using System.Linq;
using Laboratorio_3_EDII.Helper;
using System.Text;
using static Laboratorio_3_EDII.Manager.Arbol;

namespace Laboratorio_3_EDII.Manager
{
    public class Huffman : IHuffman
    {
        List<Node> frecuency = new List<Node>();

        public int Read(string direction)
        {
            var nonCarcater = 0;
            const int BufferLength = 80;
            var Buffer = new byte[BufferLength];
            var File = direction;

            using (var file = new FileStream(direction, FileMode.Open))
            {
                using (var Reader = new BinaryReader(file))
                {
                    while (Reader.BaseStream.Position != Reader.BaseStream.Length)
                    {
                        Buffer = Reader.ReadBytes(BufferLength);

                        foreach (var item in Buffer)
                        {
                            nonCarcater++;
                            Frecuency_Count(item);
                        }
                    }
                    Reader.ReadBytes(BufferLength);
                }
            }
            return nonCarcater;
        }

        //Prueba
        public int Read_Str(string Cadena)
        {
            var NoCaracter = 0;
            for (int i = 0; i < Cadena.Length; i++)
            {
                NoCaracter++;
                Frecuency_Count(Convert.ToByte(Cadena[i]));
            }
            return NoCaracter;
        }


        public void Frecuency_Count(byte Elemento)
        {
            int listPosition;
            if (frecuency.Exists(x => x.letter == Elemento))
            {
                listPosition = frecuency.FindIndex(x => x.letter == Elemento);

                Node Prueba = new Node();
                Prueba = frecuency.Find(x => x.letter == Elemento);
                frecuency.RemoveAt(listPosition);
                frecuency.Add(new Node()
                {
                    letter = Elemento,
                    readerProb = Prueba.readerProb + 1
                });
            }
            else
            {
                frecuency.Add(new Node()
                {
                    letter = Elemento,
                    readerProb = 1
                });
            }
        }
        public void Create_Tree()
        {
            List<Node> frecuencyOrder = new List<Node>();
            frecuencyOrder = frecuency.OrderBy(x => x.readerProb).ToList();

            Arbol huffmanTree = new Arbol();

            huffmanTree.EtiquetarNodo(huffmanTree.ConstruirNodo(frecuencyOrder));
            Data.Instance.codeList = huffmanTree.ListaCodigos;
        }

        public byte[] Create_Header(int noCaracteres)
        {
            double noElementos = Data.Instance.codeList.LongCount();
            string codigo = noCaracteres + "," + noElementos;
            foreach (var cosa in Data.Instance.codeList)
            {
                string p = Convert.ToString(cosa.caracter, 2);
                codigo = codigo + "," + p;
                codigo = codigo + "," + cosa.codigo;
            }
            byte[] encabezadoBytes = Encoding.ASCII.GetBytes(codigo + ",");
            return encabezadoBytes;
        }

        public List<CaracterCodigo> CrearTree()
        {
            List<Node> frecuencyOrder = new List<Node>();
            frecuencyOrder = frecuency.OrderBy(x => x.readerProb).ToList();

            Arbol ArbolH = new Arbol();

            ArbolH.EtiquetarNodo(ArbolH.ConstruirNodo(frecuencyOrder));
            return ArbolH.ListaCodigos;
        }

        public List<Node> ReturnFrecuencias()
        {
            return frecuency;
        }
    }
}
