using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRDB.Models
{
    public class Operation
    {
        public object GenerateRandomId(int Value)
        {
            var random = new Random();
            var array = "abcdefghijklmnñopqrstuvwxyz123456789";
            return new string(Enumerable.Repeat(array, Value).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
