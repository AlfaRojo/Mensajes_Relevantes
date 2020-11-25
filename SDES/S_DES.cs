using System;
using System.Collections;

namespace SDES
{
    public class S_DES
    {
        BitArray[,] Swap_Box0 = new BitArray[4, 4];
        BitArray[,] Swap_Box1 = new BitArray[4, 4];
        BitArray Master_key;

        public S_DES(string _key)
        {
            Master_key = new BitArray(10);
            for (int i = 0; i < _key.Length; i++)
            {
                Master_key[i] = string_to_bin(_key[i]);
            }

            BitArray b0 = new BitArray(2);
            b0[0] = false;
            b0[1] = false;

            BitArray b1 = new BitArray(2);
            b0[0] = false;
            b0[1] = true;

            BitArray b2 = new BitArray(2);
            b0[0] = true;
            b0[1] = false;

            BitArray b3 = new BitArray(2);
            b0[0] = true;
            b0[1] = true;

            Swap_Box0[0, 0] = b1;
            Swap_Box0[0, 1] = b0;
            Swap_Box0[0, 2] = b3;
            Swap_Box0[0, 3] = b2;

            Swap_Box0[1, 0] = b3;
            Swap_Box0[1, 1] = b2;
            Swap_Box0[1, 2] = b1;
            Swap_Box0[1, 3] = b0;

            Swap_Box0[2, 0] = b0;
            Swap_Box0[2, 1] = b2;
            Swap_Box0[2, 2] = b1;
            Swap_Box0[2, 3] = b3;

            Swap_Box0[3, 0] = b3;
            Swap_Box0[3, 1] = b1;
            Swap_Box0[3, 2] = b3;
            Swap_Box0[3, 3] = b2;
            //---------------------
            Swap_Box1[0, 0] = b0;
            Swap_Box1[0, 1] = b1;
            Swap_Box1[0, 2] = b2;
            Swap_Box1[0, 3] = b3;

            Swap_Box1[1, 0] = b2;
            Swap_Box1[1, 1] = b0;
            Swap_Box1[1, 2] = b1;
            Swap_Box1[1, 3] = b3;

            Swap_Box1[2, 0] = b3;
            Swap_Box1[2, 1] = b0;
            Swap_Box1[2, 2] = b1;
            Swap_Box1[2, 3] = b0;

            Swap_Box1[3, 0] = b2;
            Swap_Box1[3, 1] = b1;
            Swap_Box1[3, 2] = b0;
            Swap_Box1[3, 3] = b3;
            //---------------------
        }

        public byte Encrypt(byte block)
        {
            BitArray bits_block = byte_to_Bits(block);
            BitArray[] keys = Generate_Keys();
            return bits_to_Byte(IP_Inverse(Fk(Swap(Fk(IP(bits_block), keys[0])), keys[1])));
        }

        public byte Decrypt(byte block)
        {
            BitArray bits_block = byte_to_Bits(block);
            BitArray[] keys = Generate_Keys();
            return bits_to_Byte(IP_Inverse(Fk(Swap(Fk(IP(bits_block), keys[1])), keys[0])));
        }

        private BitArray byte_to_Bits(byte block)
        {
            string bits = dec_to_Bin(block);
            BitArray result = new BitArray(8);
            for (int i = 0; i < bits.Length; i++)
            {
                result[i] = string_to_bin(bits[i]);
            }
            return result;
        }

        private byte bits_to_Byte(BitArray block)
        {
            string result = "";
            for (int i = 0; i < block.Length; i++)
            {
                result += get_String(block[i]);
            }
            return bin_to_Dec(result);
        }

        private BitArray[] Generate_Keys()
        {
            BitArray[] keys = new BitArray[2];
            BitArray[] temp = Split_Block(P10(Master_key));
            keys[0] = P8(left_shift(temp[0], 1), left_shift(temp[1], 1));
            keys[1] = P8(left_shift(temp[0], 3), left_shift(temp[1], 3));
            return keys;
        }

        private string dec_to_Bin(byte num)
        {
            string ret = "";
            for (int i = 0; i < 8; i++)
            {
                if (num % 2 == 1)
                    ret = "1" + ret;
                else
                    ret = "0" + ret;
                num >>= 1;
            }
            return ret;
        }

        private byte bin_to_Dec(string binstr)
        {
            byte ret = 0;
            for (int i = 0; i < binstr.Length; i++)
            {
                ret <<= 1;
                if (binstr[i] == '1')
                    ret++;
            }
            return ret;
        }

        private string get_String(bool input)
        {
            if (input)
                return "1";
            else
                return "0";
        }

        private bool string_to_bin(char bit)
        {
            if (bit == '0')
                return false;
            else if (bit == '1')
                return true;
            else
                throw new Exception("Key should be in binary format [0,1]");
        }

        private BitArray P10(BitArray key)
        {
            BitArray permutatedArray = new BitArray(10);

            permutatedArray[0] = key[2];
            permutatedArray[1] = key[4];
            permutatedArray[2] = key[1];
            permutatedArray[3] = key[6];
            permutatedArray[4] = key[3];
            permutatedArray[5] = key[9];
            permutatedArray[6] = key[0];
            permutatedArray[7] = key[8];
            permutatedArray[8] = key[7];
            permutatedArray[9] = key[5];

            return permutatedArray;
        }

        private BitArray P8(BitArray part1, BitArray part2)
        {
            BitArray permutatedArray = new BitArray(8);

            permutatedArray[0] = part2[0];
            permutatedArray[1] = part1[2];
            permutatedArray[2] = part2[1];
            permutatedArray[3] = part1[3];
            permutatedArray[4] = part2[2];
            permutatedArray[5] = part1[4];
            permutatedArray[6] = part2[4];
            permutatedArray[7] = part2[3];

            return permutatedArray;
        }

        private BitArray P4(BitArray part1, BitArray part2)
        {
            BitArray permutatedArray = new BitArray(4);

            permutatedArray[0] = part1[1];
            permutatedArray[1] = part2[1];
            permutatedArray[2] = part2[0];
            permutatedArray[3] = part1[0];

            return permutatedArray;
        }

        private BitArray EP(BitArray input)
        {
            BitArray permutatedArray = new BitArray(8);

            permutatedArray[0] = input[3];
            permutatedArray[1] = input[0];
            permutatedArray[2] = input[1];
            permutatedArray[3] = input[2];
            permutatedArray[4] = input[1];
            permutatedArray[5] = input[2];
            permutatedArray[6] = input[3];
            permutatedArray[7] = input[0];

            return permutatedArray;
        }

        private BitArray IP(BitArray plainText)
        {
            BitArray permutatedArray = new BitArray(8);

            permutatedArray[0] = plainText[1];
            permutatedArray[1] = plainText[5];
            permutatedArray[2] = plainText[2];
            permutatedArray[3] = plainText[0];
            permutatedArray[4] = plainText[3];
            permutatedArray[5] = plainText[7];
            permutatedArray[6] = plainText[4];
            permutatedArray[7] = plainText[6];

            return permutatedArray;
        }

        private BitArray IP_Inverse(BitArray permutedText)
        {
            BitArray permutatedArray = new BitArray(8);

            permutatedArray[0] = permutedText[3];
            permutatedArray[1] = permutedText[0];
            permutatedArray[2] = permutedText[2];
            permutatedArray[3] = permutedText[4];
            permutatedArray[4] = permutedText[6];
            permutatedArray[5] = permutedText[1];
            permutatedArray[6] = permutedText[7];
            permutatedArray[7] = permutedText[5];

            return permutatedArray;
        }

        private BitArray left_shift(BitArray a, int bitNumber)
        {
            BitArray shifted = new BitArray(a.Length);
            int index = 0;
            for (int i = bitNumber; index < a.Length; i++)
            {
                shifted[index++] = a[i % a.Length];
            }
            return shifted;
        }

        private BitArray[] Split_Block(BitArray block)
        {
            BitArray[] splited = new BitArray[2];
            splited[0] = new BitArray(block.Length / 2);
            splited[1] = new BitArray(block.Length / 2);
            int index = 0;

            for (int i = 0; i < block.Length / 2; i++)
            {
                splited[0][i] = block[i];
            }
            for (int i = block.Length / 2; i < block.Length; i++)
            {
                splited[1][index++] = block[i];
            }
            return splited;
        }

        private BitArray S_Boxes(BitArray input, int no)
        {
            BitArray[,] current_S_Box;

            if (no == 1)
                current_S_Box = Swap_Box0;
            else
                current_S_Box = Swap_Box1;

            return current_S_Box[bin_to_Dec(get_String(input[0]) + get_String(input[3])),
                bin_to_Dec(get_String(input[1]) + get_String(input[2]))];
        }

        private BitArray F(BitArray right, BitArray sk)
        {
            BitArray[] temp = Split_Block(Xor(EP(right), sk));
            return P4(S_Boxes(temp[0], 1), S_Boxes(temp[1], 2));
        }

        private BitArray Fk(BitArray IP, BitArray key)
        {
            BitArray[] temp = Split_Block(IP);
            BitArray Left = Xor(temp[0], F(temp[1], key));
            BitArray joined = new BitArray(8);
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                joined[index++] = Left[i];
            }
            for (int i = 0; i < 4; i++)
            {
                joined[index++] = temp[1][i];
            }
            return joined;
        }

        private BitArray Swap(BitArray input)
        {
            BitArray switched = new BitArray(8);
            int index = 0;
            for (int i = 4; index < input.Length; i++)
            {
                switched[index++] = input[i % input.Length];
            }
            return switched;
        }

        private BitArray Xor(BitArray a, BitArray b)
        {
            return b.Xor(a);
        }
    }
}
