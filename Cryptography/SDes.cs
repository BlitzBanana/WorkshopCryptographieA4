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
            var ip = this.Ip(bits);
            var fk = this.Fk(ip, k1);
            fk = this.Sw(fk);
            fk = this.Fk(fk, k2);
            var result = this.Rip(fk);
            return Tools.BoolArrayToChar(result);
        }

        public char DecryptChar(char c)
        {
            var keys = this.GenerateKeys();
            var k1 = keys[0];
            var k2 = keys[1];
            var ip = this.Ip(Tools.CharToBoolArray(c));
            var fk = this.Fk(ip, k2);
            fk = this.Sw(fk);
            fk = this.Fk(fk, k1);
            var result = this.Rip(fk);
            return Tools.BoolArrayToChar(result);
        }

        private bool[] P10(bool[] key)
        {
            var permutedKey = new bool[10];
            permutedKey[0] = key[2];
            permutedKey[1] = key[4];
            permutedKey[2] = key[1];
            permutedKey[3] = key[6];
            permutedKey[4] = key[3];
            permutedKey[5] = key[9];
            permutedKey[6] = key[0];
            permutedKey[7] = key[8];
            permutedKey[8] = key[7];
            permutedKey[9] = key[5];
            return permutedKey;
        }

        private bool[] P8(bool[] key)
        {
            var permutedKey = new bool[8];
            permutedKey[0] = key[5];
            permutedKey[1] = key[2];
            permutedKey[2] = key[6];
            permutedKey[3] = key[3];
            permutedKey[4] = key[7];
            permutedKey[5] = key[4];
            permutedKey[6] = key[9];
            permutedKey[7] = key[8];
            return permutedKey;
        }

        private List<bool[]> GenerateKeys()
        {
            var masterKey = this.P10(this._masterKey);
            var leftKey = masterKey.Take(masterKey.Length / 2).ToArray();
            var rightKey = masterKey.Skip(masterKey.Length / 2).ToArray();

            leftKey = Tools.BoolArrayCircularLeftShift(leftKey, 1);
            rightKey = Tools.BoolArrayCircularLeftShift(rightKey, 1);

            bool[] k1 = this.P8(Tools.MergeArrays<bool>(leftKey, rightKey));

            leftKey = Tools.BoolArrayCircularLeftShift(leftKey, 2);
            rightKey = Tools.BoolArrayCircularLeftShift(rightKey, 2);

            bool[] k2 = this.P8(Tools.MergeArrays<bool>(leftKey, rightKey));

            return new List<bool[]> { k1, k2 };
        }

        private bool[] Ep(bool[] input)
        {
            var transformed = new bool[8];
            transformed[0] = input[3];
            transformed[1] = input[0];
            transformed[2] = input[1];
            transformed[3] = input[2];
            transformed[4] = input[1];
            transformed[5] = input[2];
            transformed[6] = input[3];
            transformed[7] = input[0];
            return transformed;
        }

        private bool[] Ip(bool[] plainText)
        {
            var permutedKey = new bool[8];
            permutedKey[0] = plainText[1];
            permutedKey[1] = plainText[5];
            permutedKey[2] = plainText[2];
            permutedKey[3] = plainText[0];
            permutedKey[4] = plainText[3];
            permutedKey[5] = plainText[7];
            permutedKey[6] = plainText[4];
            permutedKey[7] = plainText[6];
            return permutedKey;
        }

        private bool[] Rip(bool[] permutedText)
        {
            var permutedKey = new bool[8];
            permutedKey[0] = permutedText[3];
            permutedKey[1] = permutedText[0];
            permutedKey[2] = permutedText[2];
            permutedKey[3] = permutedText[4];
            permutedKey[4] = permutedText[6];
            permutedKey[5] = permutedText[1];
            permutedKey[6] = permutedText[7];
            permutedKey[7] = permutedText[5];
            return permutedKey;
        }

        private bool[] P4(bool[] toPermute)
        {
            var permuted = new bool[toPermute.Length];
            permuted[0] = toPermute[1];
            permuted[0] = toPermute[3];
            permuted[0] = toPermute[2];
            permuted[0] = toPermute[0];
            return permuted;
        }

        private bool[] F(bool[] input, bool[] subkey)
        {
            var key = Tools.ArrayXor(this.Ep(input), subkey);
            var left = key.Take(key.Length / 2).ToArray();
            var right = key.Skip(key.Length / 2).ToArray();
            var leftResult = this._sandbox1.Exec(new bool[] { left[0], left[3] }, new bool[] { left[1], left[2] });
            var rightResult = this._sandbox1.Exec(new bool[] { right[0], right[3] }, new bool[] { right[1], right[2] });
            var result = Tools.MergeArrays<bool>(leftResult, rightResult);
            return this.P4(result);
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
    }

    public class Sandbox
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
}
