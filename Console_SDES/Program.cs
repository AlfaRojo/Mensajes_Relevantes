using System;
using SDES;

namespace Console_SDES
{
    class Program
    {
        static void Main(string[] args)
        {
            EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\t..::SDES::..");
                Console.WriteLine("Ingrese una opción");
                Console.WriteLine("1) SDES");
                Console.WriteLine("2) Salir");
                var option = int.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Ingrese texto");
                        string encrypted_Text = encryptDecrypt.Encrypt(Console.ReadLine(), "0001100000");
                        Console.WriteLine("Texto Cifrado: " + encrypted_Text);
                        string decrypted_Text = encryptDecrypt.Decrypt(encrypted_Text, "0001100000");
                        Console.WriteLine("Texto Descifrado: " + decrypted_Text);
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.WriteLine("Saliendo...");
                        Environment.Exit(1);
                        break;
                    default:
                        Console.WriteLine("Ingese una opción válida.");
                        break;
                }
            }
        }
    }
}
