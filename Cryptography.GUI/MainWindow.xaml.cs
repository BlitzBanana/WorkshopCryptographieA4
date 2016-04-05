using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace Cryptography_GUI
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Cryptography.SDes _sDes;
        private Cryptography.BruteForce _bruteForce;

        public MainWindow()
        {
            InitializeComponent();
            this._bruteForce = new Cryptography.BruteForce();
            this._sDes = new Cryptography.SDes();
        }

        private void Encrypt(string text, string key, bool clear = true)
        {
            var encrypted = this._sDes.Encrypt(text, key);
            if (clear) this.richTextBoxEncrypted.Document.Blocks.Clear();
            this.richTextBoxEncrypted.AppendText(encrypted);
        }

        private void Decrypt(string text, string key, bool clear = true)
        {
            var decrypted = this._sDes.Decrypt(text, key);
            if (clear) this.richTextBoxClear.Document.Blocks.Clear();
            this.richTextBoxClear.AppendText(decrypted + "\n");
            Console.WriteLine("Decrypted: " + decrypted);
        }

        private void DecryptBruteforce(string text, string partialKey)
        {
            Console.WriteLine("Decryption with bruteforce:");

            foreach (var result in this._bruteForce.Exec(partialKey, 10))
            {
                this.Decrypt(text, result, false);
            }
        }

        private void buttonEncrypt_Click(object sender, RoutedEventArgs e)
        {
            var clear = new TextRange(this.richTextBoxClear.Document.ContentStart, this.richTextBoxClear.Document.ContentEnd).Text;
            var key = this.textBoxKey.Text;
            this.Encrypt(clear, key);
        }

        private void buttonDecrypt_Click(object sender, RoutedEventArgs e)
        {
            var encrypted = new TextRange(this.richTextBoxEncrypted.Document.ContentStart, this.richTextBoxEncrypted.Document.ContentEnd).Text;
            var key = this.textBoxKey.Text;
            if (key.Length > 9)
                this.Decrypt(encrypted, key);
            else
                this.DecryptBruteforce(encrypted, key);
        }
    }
}
