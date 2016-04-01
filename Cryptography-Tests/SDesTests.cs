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
            var sdes = new Cryptography.SDes(key);
            var encrypted = sdes.Encrypt(clear);
            var decrypted = sdes.Decrypt(encrypted);
            Assert.AreNotEqual(clear, encrypted);
            Assert.AreNotEqual(encrypted, decrypted);
            Assert.AreEqual(clear, decrypted);
        }
    }
}
