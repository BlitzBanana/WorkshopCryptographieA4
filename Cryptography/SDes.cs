using System.Collections.Generic;
using System.Linq;

namespace Cryptography
{
    public class SDes
    {
        private bool[] _masterKey;
        private Sandbox _sandbox1;
        private Sandbox _sandbox2;

        public SDes()
        {
            this._sandbox1 = new Sandbox(new bool[,][]
            {
                { new [] { false, false }, new [] { true, true }, new [] { false, false }, new [] { false, false } },
                { new [] { false, false }, new [] { false, false }, new [] { false, false }, new [] { true, true } },
                { new [] { true, true }, new [] { false, false }, new [] { false, false }, new [] { false, false } },
                { new [] { false, false }, new [] { false, false }, new [] { false, false }, new [] { false, false } }
            });
            this._sandbox2 = new Sandbox(new bool[,][]
            {
                { new [] { true, true }, new [] { false, false }, new [] { false, false }, new [] { false, false } },
                { new [] { false, false }, new [] { true, true }, new [] { false, false }, new [] { false, false } },
                { new [] { false, false }, new [] { true, true }, new [] { false, false }, new [] { true, true } },
                { new [] { false, false }, new [] { false, false }, new [] { true, true }, new [] { false, false } }
            });
        }

        public string Encrypt(string sentence, string masterKey)
        {
            this._masterKey = masterKey.Take(10).Select(x => x == '0' ? false : true).ToArray();
            var result = new List<char>();
            foreach (char c in sentence)
                result.Add(this.EncryptChar(c));
            return string.Concat(result);
        }

        public string Decrypt(string sentence, string masterKey)
        {
            this._masterKey = masterKey.Take(10).Select(x => x == '0' ? false : true).ToArray();
            var result = new List<char>();
            foreach (var c in sentence)
                result.Add(this.DecryptChar(c));
            return string.Concat(result);
        }

        public char EncryptChar(char character)
        {
            var bits = Tools.CharToBoolArray(character);
            var keys = this.GenerateKeys();
            var k1 = keys[0];
            var k2 = keys[1];
            var ip = SDes.Permutations.Ip(bits);
            var fk = this.Fk(ip, k1);
            fk = this.Swap(fk);
            fk = this.Fk(fk, k2);
            var result = SDes.Permutations.Rip(fk);
            return Tools.BoolArrayToChar(result);
        }

        public char DecryptChar(char character)
        {
            var keys = this.GenerateKeys();
            var k1 = keys[0];
            var k2 = keys[1];
            var ip = SDes.Permutations.Ip(Tools.CharToBoolArray(character));
            var fk = this.Fk(ip, k2);
            fk = this.Swap(fk);
            fk = this.Fk(fk, k1);
            var result = SDes.Permutations.Rip(fk);
            return Tools.BoolArrayToChar(result);
        }

        private List<bool[]> GenerateKeys()
        {
            var masterKey = SDes.Permutations.P10(this._masterKey);
            var leftKey = masterKey.Take(masterKey.Length / 2).ToArray();
            var rightKey = masterKey.Skip(masterKey.Length / 2).ToArray();

            leftKey = Tools.BoolArrayCircularLeftShift(leftKey, 1);
            rightKey = Tools.BoolArrayCircularLeftShift(rightKey, 1);

            bool[] k1 = SDes.Permutations.P8(Tools.MergeArrays<bool>(leftKey, rightKey));

            leftKey = Tools.BoolArrayCircularLeftShift(leftKey, 2);
            rightKey = Tools.BoolArrayCircularLeftShift(rightKey, 2);

            bool[] k2 = SDes.Permutations.P8(Tools.MergeArrays<bool>(leftKey, rightKey));

            return new List<bool[]> { k1, k2 };
        }

        private bool[] F(bool[] input, bool[] subkey)
        {
            var key = Tools.ArrayXor(SDes.Permutations.Ep(input), subkey);
            var left = key.Take(key.Length / 2).ToArray();
            var right = key.Skip(key.Length / 2).ToArray();
            var leftResult = this._sandbox1.Exec(new bool[] { left[0], left[3] }, new bool[] { left[1], left[2] });
            var rightResult = this._sandbox1.Exec(new bool[] { right[0], right[3] }, new bool[] { right[1], right[2] });
            var result = Tools.MergeArrays<bool>(leftResult, rightResult);
            return SDes.Permutations.P4(result);
        }

        private bool[] Fk(bool[] bits, bool[] key)
        {
            var left = bits.Take(bits.Length / 2).ToArray();
            var right = bits.Skip(bits.Length / 2).ToArray();
            var result = Tools.ArrayXor(left, this.F(right, key));
            return Tools.MergeArrays<bool>(result, right);
        }

        private bool[] Swap(bool[] input)
        {
            var left = input.Take(input.Length / 2).ToArray();
            var right = input.Skip(input.Length / 2).ToArray();
            return Tools.MergeArrays<bool>(right, left);
        }

        class Sandbox
        {
            private bool[,][] _matrix;

            public Sandbox(bool[,][] matrix)
            {
                this._matrix = matrix;
            }

            public bool[] Exec(bool[] left, bool[] right)
            {
                var x = this.CoupleToInt(left);
                var y = this.CoupleToInt(right);
                return this._matrix[x, y];
            }

            private int CoupleToInt(bool[] booleans)
            {
                var left = booleans[0];
                var right = booleans[1];
                if (left == false && right == false) return 0;
                if (left == false && right == true) return 1;
                if (left == true && right == false) return 2;
                if (left == true && right == true) return 3;
                return -1;
            }
        }

        class Permutations
        {
            private static bool[] Permute(bool[] toPermute, int[] permutation)
            {
                var permuted = new bool[permutation.Length];
                for (int i = 0; i < permutation.Length; i++)
                    permuted[i] = toPermute[permutation[i] - 1];
                return permuted;
            }

            internal static bool[] P10(bool[] toPermute)
            {
                var permutation = new int[] { 3, 5, 2, 7, 4, 10, 1, 9, 8, 6 };
                return Permute(toPermute, permutation);
            }

            internal static bool[] P8(bool[] toPermute)
            {
                var permutation = new int[] { 6, 3, 7, 4, 8, 5, 10, 9 };
                return Permute(toPermute, permutation);
            }

            internal static bool[] Ep(bool[] toPermute)
            {
                var permutation = new int[] { 4, 1, 2, 3, 2, 3, 4, 1 };
                return Permute(toPermute, permutation);
            }

            internal static bool[] Ip(bool[] toPermute)
            {
                var permutation = new int[] { 2, 6, 3, 1, 4, 8, 5, 7 };
                return Permute(toPermute, permutation);
            }

            internal static bool[] Rip(bool[] toPermute)
            {
                var permutation = new int[] { 4, 1, 3, 5, 7, 2, 8, 6 };
                return Permute(toPermute, permutation);
            }

            internal static bool[] P4(bool[] toPermute)
            {
                var permutation = new int[] { 2, 4, 3, 1 };
                return Permute(toPermute, permutation);
            }
        }
    }
}
