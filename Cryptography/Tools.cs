using System;
using System.Collections.Generic;
using System.Linq;

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
            for (int i = 0; i < left.Length; i++)
                results[i] = left[i] ^ right[i];
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
    }
}
