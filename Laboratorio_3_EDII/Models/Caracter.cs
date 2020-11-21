using System;
namespace Laboratorio_3_EDII.Models
{
    public class Caracter : IComparable
    {
        public byte textCaracter { get; set; }
        public int frequency { get; set; }
        public int index { get; set; }
        public string textBin { get; set; }
        public bool interactionCaracter { get; set; }
        public bool caracterToUse { get; set; }


        public Caracter()
        {
            interactionCaracter = false;
            caracterToUse = false;
        }

        public int CompareTo(object obj)
        {
            var vComparador = (Caracter)obj;
            return textCaracter.CompareTo(vComparador.textCaracter);
        }
    }
}
