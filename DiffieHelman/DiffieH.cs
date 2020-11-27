using System;
using System.Numerics;

namespace DiffieHelman
{
    public class DiffieH
    {
        public static BigInteger DiffieHelmannAlgorithm(int numberA, int numberB)
        {
            Random rnd = new Random();
            BigInteger numberG = rnd.Next(200, 1022);
            BigInteger numberP = Get_Random_Prime();
            BigInteger numberFromA = BigInteger.ModPow(numberG, (numberA), numberP);
            BigInteger numberFromB = BigInteger.ModPow(numberG, (numberB), numberP);
            BigInteger SecretKeyFromA = BigInteger.ModPow(numberFromB, (numberA), numberP);
            BigInteger SecretKeyFromB = BigInteger.ModPow(numberFromA, (numberB), numberP);
            if (SecretKeyFromA == SecretKeyFromB)
            {
                return SecretKeyFromA;
            }
            return 0;
        }
        private static int Get_Random_Prime()
        {
            var rand = new Random();
            int value = rand.Next(50, 547);
            for (int i = value; i < 300; i++)
            {
                if (isPrime(i))
                {
                    return i;
                }
            }
            return 3;
        }

        private static bool isPrime(int number)
        {
            for (int i = 37; i < number; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
