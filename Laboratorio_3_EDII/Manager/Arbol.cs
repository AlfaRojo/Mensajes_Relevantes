using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Laboratorio_3_EDII.IService;

namespace Laboratorio_3_EDII.Manager
{
    public class Arbol
    {
        public Node Raiz { get; set; }
        public List<CaracterCodigo> ListaCodigos = new List<CaracterCodigo>();

        /// <summary>
        /// Crea el nodo para ser usado
        /// </summary>
        /// <param name="derecho"></param>
        /// <param name="izquierdo"></param>
        /// <returns></returns>
        public Node NodoPadre(Node derecho, Node izquierdo)
        {
            Node Padre = new Node(0, derecho.readerProb + izquierdo.readerProb);
            Padre.derecho = derecho;
            Padre.izquierdo = izquierdo;
            return Padre;
        }

        /// <summary>
        /// Distribuye los valores dentro de un nodo
        /// </summary>
        /// <param name="ListaProbabilidades"></param>
        /// <returns></returns>
        public Node ConstruirNodo(List<Node> ListaProbabilidades)
        {
            List<Node> LPrincipal = ListaProbabilidades;
            List<Node> LSecundaria = new List<Node>();

            while (LPrincipal.Count > 2)
            {
                LSecundaria = LPrincipal;
                Node nuevoNodo = NodoPadre(LSecundaria[0], LSecundaria[1]);
                LSecundaria.RemoveRange(0, 2);
                LSecundaria.Add(nuevoNodo);
                LPrincipal = LSecundaria.OrderBy(o => o.readerProb).ToList();
            }
            return Raiz = NodoPadre(LPrincipal[0], LPrincipal[1]);
        }

        /// <summary>
        /// Realiza la asignación de cógidos prefijos
        /// </summary>
        /// <param name="Raiz"></param>
        public void EtiquetarNodo(Node Raiz)
        {
            string Etiquette = Raiz.etiqueta;
            if (Raiz.izquierdo != null)
            {
                Raiz.izquierdo.etiqueta = Etiquette + "0";
                EtiquetarNodo(Raiz.izquierdo);
            }
            if (Raiz.derecho != null)
            {
                //Ó un 1 cuando es hijo derecho
                Raiz.derecho.etiqueta = Etiquette + "1";
                EtiquetarNodo(Raiz.derecho);
            }
            if (Raiz.derecho == null && Raiz.izquierdo == null)
            {
                CaracterCodigo nuevoCaracter = new CaracterCodigo(Raiz.letter, Raiz.etiqueta);
                ListaCodigos.Add(nuevoCaracter);
            }
        }

        public class Node
        {
            public string etiqueta = "";
            public double readerProb;
            public byte letter;
            public Node izquierdo;
            public Node derecho;
            public Node()
            {

            }
            public Node(byte car, double prob)
            {
                readerProb = prob;
                letter = car;
            }
        }
        public class CaracterCodigo
        {
            public string codigo;
            public byte caracter;

            public CaracterCodigo()
            {

            }
            public CaracterCodigo(byte car, string cod)
            {
                caracter = car;
                codigo = cod;
            }
        }
    }
}
