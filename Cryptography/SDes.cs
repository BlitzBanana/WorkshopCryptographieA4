using System.Collections.Generic;
using System.Linq;

namespace Cryptography
{
    public class SDes
    {
        private bool[] _masterKey;
        private Sandbox _sandbox1;
        private Sandbox _sandbox2;

        public SDes(string masterKey)
            :this(masterKey.Take(10).Select(x => x == '0' ? false : true).ToArray())
        { }

        public SDes(bool[] masterKey)
        {
            this._masterKey = masterKey.Take(10).ToArray();
            this._sandbox1 = new Sandbox(new bool[,][]
            {
                { new [] { false, true }, new [] { false, false }, new [] { true, true }, new [] { true, false } },
                { new [] { true, true }, new [] { true, false }, new [] { false, true }, new [] { false, false } },
                { new [] { false, false }, new [] { true, false }, new [] { false, true }, new [] { true, true } },
                { new [] { true, true }, new [] { false, true }, new [] { true, true }, new [] { true, false } }
            });
            this._sandbox2 = new Sandbox(new bool[,][]
            {
                { new [] { false, false }, new [] { false, true }, new [] { true, false }, new [] { true, true } },
                { new [] { true, false }, new [] { false, false }, new [] { false, true }, new [] { true, true } },
                { new [] { true, true }, new [] { false, false }, new [] { false, true }, new [] { false, false } },
                { new [] { true, false }, new [] { false, true }, new [] { false, false }, new [] { true, true } }
            });
        }

        public string Encrypt(string sentence)
        {
            var result = new List<char>();
            foreach (char c in sentence)
                result.Add(this.EncryptChar(c));
            return string.Concat(result);
        }

        public string Decrypt(string sentence)
        {
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
            fk = this.Sw(fk);
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
            fk = this.Sw(fk);
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

        private bool[] Sw(bool[] input)
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

            private int CoupleToInt(bool[] bools)
            {
                var one = bools[0];
                var two = bools[1];
                if (one == false && two == false) return 0;
                if (one == false && two == true) return 1;
                if (one == true && two == false) return 2;
                if (one == true && two == true) return 3;
                return -1;
            }
        }

        class Permutations
        {
            internal static bool[] P10(bool[] toPermute)
            {
                var permuted = new bool[10];
                permuted[0] = toPermute[2];
                permuted[1] = toPermute[4];
                permuted[2] = toPermute[1];
                permuted[3] = toPermute[6];
                permuted[4] = toPermute[3];
                permuted[5] = toPermute[9];
                permuted[6] = toPermute[0];
                permuted[7] = toPermute[8];
                permuted[8] = toPermute[7];
                permuted[9] = toPermute[5];
                return permuted;
            }

            internal static bool[] P8(bool[] toPermute)
            {
                var permuted = new bool[8];
                permuted[0] = toPermute[5];
                permuted[1] = toPermute[2];
                permuted[2] = toPermute[6];
                permuted[3] = toPermute[3];
                permuted[4] = toPermute[7];
                permuted[5] = toPermute[4];
                permuted[6] = toPermute[9];
                permuted[7] = toPermute[8];
                return permuted;
            }

            internal static bool[] Ep(bool[] toPermute)
            {
                var permuted = new bool[8];
                permuted[0] = toPermute[3];
                permuted[1] = toPermute[0];
                permuted[2] = toPermute[1];
                permuted[3] = toPermute[2];
                permuted[4] = toPermute[1];
                permuted[5] = toPermute[2];
                permuted[6] = toPermute[3];
                permuted[7] = toPermute[0];
                return permuted;
            }

            internal static bool[] Ip(bool[] toPermute)
            {
                var permuted = new bool[8];
                permuted[0] = toPermute[1];
                permuted[1] = toPermute[5];
                permuted[2] = toPermute[2];
                permuted[3] = toPermute[0];
                permuted[4] = toPermute[3];
                permuted[5] = toPermute[7];
                permuted[6] = toPermute[4];
                permuted[7] = toPermute[6];
                return permuted;
            }

            internal static bool[] Rip(bool[] toPermute)
            {
                var permuted = new bool[8];
                permuted[0] = toPermute[3];
                permuted[1] = toPermute[0];
                permuted[2] = toPermute[2];
                permuted[3] = toPermute[4];
                permuted[4] = toPermute[6];
                permuted[5] = toPermute[1];
                permuted[6] = toPermute[7];
                permuted[7] = toPermute[5];
                return permuted;
            }

            internal static bool[] P4(bool[] toPermute)
            {
                var permuted = new bool[toPermute.Length];
                permuted[0] = toPermute[1];
                permuted[0] = toPermute[3];
                permuted[0] = toPermute[2];
                permuted[0] = toPermute[0];
                return permuted;
            }
        }
    }
}
