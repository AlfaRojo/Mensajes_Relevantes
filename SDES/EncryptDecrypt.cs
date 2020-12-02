using System;
using System.IO;
using System.Text;

namespace SDES
{
    public class EncryptDecrypt
    {
        public string Encrypt(string plainText, string _key)
        {
            if (_key.Length < 10)
            {
                _key = Complete_Bin(_key);
            }

            S_DES s_DES = new S_DES(_key);
            string password = string.Empty;
            using (BinaryReader br = new BinaryReader(new MemoryStream(Encoding.Default.GetBytes(plainText))))
            {
                int blocksize = 4 * 1024;
                int iteration_number;
                if (plainText.Length < blocksize)
                {
                    iteration_number = 1;
                }
                else if ((plainText.Length % blocksize) == 0)
                {
                    iteration_number = plainText.Length / blocksize;
                }
                else
                {
                    iteration_number = (plainText.Length / blocksize) + 1;
                }
                while (iteration_number-- > 0)
                {
                    if (iteration_number == 0)
                    {
                        blocksize = plainText.Length % blocksize;
                    }
                    byte[] input = br.ReadBytes(blocksize);
                    byte[] output = new byte[input.Length];
                    for (int i = 0; i < output.Length; i++)
                    {
                        output[i] = s_DES.Encrypt(input[i]);
                    }
                    password = Concatened(output);

                }
            }
            return password;
        }
        public string Decrypt(string plainText, string _key)
        {

            if (_key.Length < 10)
            {
                _key = Complete_Bin(_key);
            }

            string password = string.Empty;
            S_DES s_DES = new S_DES(_key);
            using (BinaryReader br = new BinaryReader(new MemoryStream(Encoding.Default.GetBytes(plainText))))
            {
                int blocksize = 4 * 1024;
                int iteration_number;
                if (plainText.Length < blocksize)
                {
                    iteration_number = 1;
                }
                else if ((plainText.Length % blocksize) == 0)
                {
                    iteration_number = plainText.Length / blocksize;
                }
                else
                {
                    iteration_number = (plainText.Length / blocksize) + 1;
                }
                while (iteration_number-- > 0)
                {
                    if (iteration_number == 0)
                    {
                        blocksize = plainText.Length % blocksize;
                    }
                    byte[] input = SplitByte(plainText);
                    byte[] output = new byte[input.Length];
                    for (int i = 0; i < output.Length; i++)
                    {
                        output[i] = s_DES.Decrypt(input[i]);
                    }
                    password += Encoding.Default.GetString(output);
                }
            }
            return password;
        }

        private string Complete_Bin(string _key)
        {
            _key = _key.PadLeft(10, '0');
            return _key;
        }

        private string Concatened(byte[] _bytes)
        {
            var _Concatened = "";

            for (int i = 0; i < _bytes.Length; i++)
            {
                var value = (int)_bytes[i];
                if(i < _bytes.Length - 1) { _Concatened += $"{value}|"; }
                else { _Concatened += $"{value}"; }
                
            }
            return _Concatened;
        }

        private byte[] SplitByte(string pharasse)
        {
            var _splitString = pharasse.Split('|');
            byte[] _bytes = new byte[_splitString.Length];
            for (int i = 0; i < _splitString.Length; i++)
            {
                _bytes[i] = Convert.ToByte(_splitString[i]);
            }
            return _bytes;
        }
    }
}