﻿using System;
using System.Numerics;

namespace DiffieHelman
{
    public class DiffieH
    {
        public static BigInteger DiffieHelmannAlgorithm(int numberA, int numberB)
        {
            Random rnd = new Random();
            BigInteger numberG = 1000;
            BigInteger numberP = 887;
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
       
    }
}
