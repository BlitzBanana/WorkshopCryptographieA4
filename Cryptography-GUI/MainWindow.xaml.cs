using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Cryptography_GUI
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var sdes = new Cryptography.SDes("0101010101");
        }

        private bool[] ParseKey(string stringKey)
        {
            var key = new List<bool>();
            foreach (var c in stringKey)
                key.Add(c == '0' ? false : true);
            return key.ToArray();
        }

        private void Encrypt(string text, bool[] key, bool clear = true)
        {
            var sDes = new Cryptography.SDes(key.ToArray());
            var encrypted = sDes.Encrypt(text);
            if (clear) this.richTextBoxEncrypted.Document.Blocks.Clear();
            this.richTextBoxEncrypted.AppendText(encrypted);
        }

        private void Decrypt(string text, bool[] key, bool clear = true)
        {
            var sDes = new Cryptography.SDes(key.ToArray());
            var decrypted = sDes.Decrypt(text);
            if (clear) this.richTextBoxClear.Document.Blocks.Clear();
            this.richTextBoxClear.AppendText(decrypted + "\n");
        }

        private void DecryptBruteforce(string text, bool[] partialKey)
        {
            var key = new bool[10];
            for (int i = 0; i < 32; i++)
            {
                var chars = Cryptography.Tools.GetIntBinaryString(i).ToCharArray();
                chars = chars.Skip(chars.Length - 5).ToArray();
                var genKey = this.ParseKey(string.Concat(chars));
                Console.WriteLine(string.Concat(chars) + "\n");
                var fullKey = Cryptography.Tools.MergeArrays<bool>(partialKey, genKey);
                this.Decrypt(text, fullKey, false);
            }
        }

        private void buttonEncrypt_Click(object sender, RoutedEventArgs e)
        {
            var clear = new TextRange(this.richTextBoxClear.Document.ContentStart, this.richTextBoxClear.Document.ContentEnd).Text;
            var key = this.textBoxKey.Text;
            this.Encrypt(clear, this.ParseKey(key));
        }

        private void buttonDecrypt_Click(object sender, RoutedEventArgs e)
        {
            var encrypted = new TextRange(this.richTextBoxEncrypted.Document.ContentStart, this.richTextBoxEncrypted.Document.ContentEnd).Text;
            var key = this.textBoxKey.Text;
            if (key.Length > 9)
                this.Decrypt(encrypted, this.ParseKey(key));
            else
                this.DecryptBruteforce(encrypted, this.ParseKey(key));
        }
    }
}
