using Laboratorio_3_EDII.Helper;
using System;
using System.Collections.Generic;

namespace Laboratorio_3_EDII.Models
{
    public class Nodo<T>
    {
        public int index;
        public int father;
        public int numberValues;
        public List<int> children = new List<int>();
        public List<T> values = new List<T>();
        static int lenght = 300;
        public Nodo(int dad)
        {
            if (dad == 0)
            {
                numberValues = (4 * (Data.Instance.grade - 1)) / 3;
            }
            else
            {
                numberValues = Data.Instance.grade - 1;
            }
            this.father = dad;
        }
    }
}
