using System;
using System.Linq;
using System.Collections.Generic;

namespace Cryptography
{
    public static class Tools
    {
        public static bool[] BoolArrayCircularLeftShift(bool[] array, int offset)
        {
            var shiftedArray = new bool[array.Length];
            Buffer.BlockCopy(array, offset * sizeof(bool), shiftedArray, 0, (array.Length - offset) * sizeof(bool));
            Buffer.BlockCopy(array, 0, shiftedArray, (array.Length - offset) * sizeof(bool), offset * sizeof(bool));
            return shiftedArray;
        }

        public static bool[] ArrayXor(bool[] left, bool[] right)
        {
            var results = new bool[left.Length];
            System.Threading.Tasks.Parallel.For(0, left.Length, i => {
                results[i] = left[i] ^ right[i];
            });
            return results;
        }

        public static T[] MergeArrays<T>(T[] left, T[] right)
        {
            var merged = new T[left.Length + right.Length];
            left.CopyTo(merged, 0);
            right.CopyTo(merged, left.Length);
            return merged;
        }

        public static bool[] CharToBoolArray(char character)
        {
            var bitArray = new System.Collections.BitArray(System.Text.Encoding.Default.GetBytes(new[] { character }));
            bool[] result = new bool[bitArray.Length];
            for (int i = 0; i < bitArray.Length; i++)
                result[i] = bitArray[i];
            return result;
        }

        public static char BoolArrayToChar(bool[] bits)
        {
            var bitArray = new System.Collections.BitArray(bits);
            byte[] bytes = new byte[bitArray.Length];
            bitArray.CopyTo(bytes, 0);
            return System.Text.Encoding.Default.GetString(bytes).ToCharArray().First();
        }

        public static string GetIntBinaryString(int n)
        {
            var b = new char[32];
            var pos = 31;
            var i = 0;

            while (i < 32)
            {
                if ((n & (1 << i)) != 0)
                    b[pos] = '1';
                else
                    b[pos] = '0';
                pos--;
                i++;
            }
            return new string(b);
        }
        
        public static bool[] ParseKey(string stringKey)
        {
            var key = new List<bool>();
            foreach (var c in stringKey)
                key.Add(c == '0' ? false : true);
            return key.ToArray();
        }
    }
}
