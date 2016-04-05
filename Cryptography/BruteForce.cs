using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    public class BruteForce
    {
        private PartialKey Key { get; set; }
        private int _i = 0;
        private int _maxIteration;

        public IEnumerable<string> Exec(string partialKey, int maxDigit)
        {
            this.Key = new PartialKey(partialKey, maxDigit);

            this._maxIteration = Convert.ToInt32(Math.Pow(2, this.Key.CountMissingCharacters()));

            for (this._i = 0; this._i < this._maxIteration; this._i++)
            {
                yield return this.Key.Replace(this.GetBinary(this._i));
            }
        }

        private string GetBinary(int value)
        {
            string result = Convert.ToString(value, 2);

            if (result.Length < this.Key.MaxDigit)
                for (int i = result.Length; i < this.Key.NbrMissingCharacters; i++)
                    result = result.Insert(0, "0");

            return result;
        }

        private class PartialKey
        {
            public string Key { get; set; }

            public int MaxDigit { get; set; }

            public int NbrMissingCharacters { get; set; }

            public PartialKey(string partialKey, int maxDigit)
            {
                this.MaxDigit = maxDigit;
                this.Key = partialKey;
            }

            public int CountMissingCharacters()
            {
                int result = 0;

                result += Key.Where(x => x == '*').Count();
                result += this.MaxDigit - Key.Length;

                this.NbrMissingCharacters = result;

                return this.NbrMissingCharacters;
            }

            public string Replace(string missingCharacters)
            {
                if (missingCharacters.Length != this.NbrMissingCharacters)
                    throw new Exception("At least one character is missing");

                StringBuilder result = new StringBuilder();

                for (int i = 0; i < this.Key.Length; i++)
                {
                    result.Append(this.Key[i] == '*' ? missingCharacters[i] : this.Key[i]);
                }

                result.Append(missingCharacters.Substring(Key.Where(x => x == '*').Count()));

                return result.ToString();
            }
        }
    }
}
