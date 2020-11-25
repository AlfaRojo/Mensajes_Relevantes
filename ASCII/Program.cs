using System;
using System.Collections.Generic;
using MRDB.Models;

namespace ASCII
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Charecter> Charecters = new List<Charecter>();

            List<Charecter> Charecters2 = new List<Charecter>();

            for (int i = 0; i < 256; i++)
            {
                Charecter charecter = new Charecter();
                charecter.Decimal = i;
                charecter.Character = Convert.ToString(Convert.ToChar(i));
                
                Charecters.Add(charecter);
            }


            foreach (var item in Charecters)
            {
                Console.WriteLine(item.Decimal + ", " + item.Character);
            }

            Console.ReadLine();
        }
    }
}
