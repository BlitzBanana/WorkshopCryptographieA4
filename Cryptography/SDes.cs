using System.Collections.Generic;
using System.Linq;

namespace Cryptography
{
    public class SDes
    {
        private readonly Sandbox _sandbox1;
        private readonly Sandbox _sandbox2;

        public SDes()
        {
            this._sandbox1 = new Sandbox(new[,]
            {
                { new [] { false, false }, new [] { true, true }, new [] { false, false }, new [] { false, false } },
                { new [] { false, false }, new [] { false, false }, new [] { false, false }, new [] { true, true } },
                { new [] { true, true }, new [] { false, false }, new [] { false, false }, new [] { false, false } },
                { new [] { false, false }, new [] { false, false }, new [] { false, false }, new [] { false, false } }
            });
            this._sandbox2 = new Sandbox(new[,]
            {
                { new [] { true, true }, new [] { false, false }, new [] { false, false }, new [] { false, false } },
                { new [] { false, false }, new [] { true, true }, new [] { false, false }, new [] { false, false } },
                { new [] { false, false }, new [] { true, true }, new [] { false, false }, new [] { true, true } },
                { new [] { false, false }, new [] { false, false }, new [] { true, true }, new [] { false, false } }
            });
        }

        public string Encrypt(string sentence, string masterKey)
        {
            var key = this.ParseKey(masterKey);
            var result = sentence.Select(c => this.EncryptChar(c, key)).ToList();
            return string.Concat(result);
        }

        public string Decrypt(string sentence, string masterKey)
        {
            var key = this.ParseKey(masterKey);
            var result = sentence.Select(c => this.DecryptChar(c, key)).ToList();
            return string.Concat(result);
        }

        private char EncryptChar(char character, bool[] key)
        {
            var bits = Tools.CharToBoolArray(character);
            var keys = this.GenerateKeys(key);
            var k1 = keys[0];
            var k2 = keys[1];
            var ip = Permutations.Ip(bits);
            var fk = this.Fk(ip, k1);
            fk = this.Swap(fk);
            fk = this.Fk(fk, k2);
            var result = Permutations.Rip(fk);
            return Tools.BoolArrayToChar(result);
        }

        private char DecryptChar(char character, bool[] key)
        {
            var keys = this.GenerateKeys(key);
            var k1 = keys[0];
            var k2 = keys[1];
            var ip = Permutations.Ip(Tools.CharToBoolArray(character));
            var fk = this.Fk(ip, k2);
            fk = this.Swap(fk);
            fk = this.Fk(fk, k1);
            var result = Permutations.Rip(fk);
            return Tools.BoolArrayToChar(result);
        }

        private bool[] ParseKey(string key)
        {
            return key.Take(10).Select(x => x != '0').ToArray();
        }

        private List<bool[]> GenerateKeys(bool[] key)
        {
            var masterKey = Permutations.P10(key);
            var leftKey = masterKey.Take(masterKey.Length / 2).ToArray();
            var rightKey = masterKey.Skip(masterKey.Length / 2).ToArray();

            leftKey = Tools.BoolArrayCircularLeftShift(leftKey, 1);
            rightKey = Tools.BoolArrayCircularLeftShift(rightKey, 1);

            bool[] k1 = Permutations.P8(Tools.MergeArrays(leftKey, rightKey));

            leftKey = Tools.BoolArrayCircularLeftShift(leftKey, 2);
            rightKey = Tools.BoolArrayCircularLeftShift(rightKey, 2);

            bool[] k2 = Permutations.P8(Tools.MergeArrays(leftKey, rightKey));

            return new List<bool[]> { k1, k2 };
        }

        private bool[] F(bool[] input, bool[] subkey)
        {
            var key = Tools.ArrayXor(Permutations.Ep(input), subkey);
            var left = key.Take(key.Length / 2).ToArray();
            var right = key.Skip(key.Length / 2).ToArray();
            var leftResult = this._sandbox1.Exec(new[] { left[0], left[3] }, new[] { left[1], left[2] });
            var rightResult = this._sandbox2.Exec(new[] { right[0], right[3] }, new[] { right[1], right[2] });
            var result = Tools.MergeArrays(leftResult, rightResult);
            return Permutations.P4(result);
        }

        private bool[] Fk(bool[] bits, bool[] key)
        {
            var left = bits.Take(bits.Length / 2).ToArray();
            var right = bits.Skip(bits.Length / 2).ToArray();
            var result = Tools.ArrayXor(left, this.F(right, key));
            return Tools.MergeArrays(result, right);
        }

        private bool[] Swap(bool[] input)
        {
            var left = input.Take(input.Length / 2).ToArray();
            var right = input.Skip(input.Length / 2).ToArray();
            return Tools.MergeArrays(right, left);
        }

        class Sandbox
        {
            private readonly bool[,][] _matrix;

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
                if (left == false) return 1;
                if (right == false) return 2;
                return 3;
            }
        }

        static class Permutations
        {
            private static bool[] Permute(bool[] toPermute, IReadOnlyList<int> permutation)
            {
                var permuted = new bool[permutation.Count];
                for (var i = 0; i < permutation.Count; i++)
                    permuted[i] = toPermute[permutation[i] - 1];
                return permuted;
            }

            internal static bool[] P10(bool[] toPermute)
            {
                var permutation = new[] { 3, 5, 2, 7, 4, 10, 1, 9, 8, 6 };
                return Permute(toPermute, permutation);
            }

            internal static bool[] P8(bool[] toPermute)
            {
                var permutation = new[] { 6, 3, 7, 4, 8, 5, 10, 9 };
                return Permute(toPermute, permutation);
            }

            internal static bool[] Ep(bool[] toPermute)
            {
                var permutation = new[] { 4, 1, 2, 3, 2, 3, 4, 1 };
                return Permute(toPermute, permutation);
            }

            internal static bool[] Ip(bool[] toPermute)
            {
                var permutation = new[] { 2, 6, 3, 1, 4, 8, 5, 7 };
                return Permute(toPermute, permutation);
            }

            internal static bool[] Rip(bool[] toPermute)
            {
                var permutation = new[] { 4, 1, 3, 5, 7, 2, 8, 6 };
                return Permute(toPermute, permutation);
            }

            internal static bool[] P4(bool[] toPermute)
            {
                var permutation = new[] { 2, 4, 3, 1 };
                return Permute(toPermute, permutation);
            }
        }
    }
}
