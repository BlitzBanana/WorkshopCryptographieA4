using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cryptography_Tests
{
    [TestClass]
    public class SDesTests
    {
        [TestMethod]
        public void EncryptDecrypt()
        {
            var clear = "Hello World !";
            var key = "0101010101";
            var sdes = new Cryptography.SDes();
            var encrypted = sdes.Encrypt(clear, key);
            var decrypted = sdes.Decrypt(encrypted, key);
            Assert.AreNotEqual(clear, encrypted);
            Assert.AreNotEqual(encrypted, decrypted);
            Assert.AreEqual(clear, decrypted);
        }
    }
}
