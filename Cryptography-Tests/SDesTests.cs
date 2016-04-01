using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cryptography_Tests
{
    [TestClass]
    public class SDesTests
    {
        [TestMethod]
        public void SandboxTest()
        {
            bool[] result;
            var sandbox = new Cryptography.Sandbox(new bool[,][]
            {
                { new [] { false, true }, new [] { false, false }, new [] { true, true }, new [] { true, false } },
                { new [] { true, true }, new [] { true, false }, new [] { false, true }, new [] { false, false } },
                { new [] { false, false }, new [] { true, false }, new [] { false, true }, new [] { true, true } },
                { new [] { true, true }, new [] { false, true }, new [] { true, true }, new [] { true, false } }
            });

            result = sandbox.Exec(new bool[] { false, false }, new bool[] { true, false });
            Assert.AreEqual(result[0], true);
            Assert.AreEqual(result[1], true);

            result = sandbox.Exec(new bool[] { true, true }, new bool[] { false, false });
            Assert.AreEqual(result[0], true);
            Assert.AreEqual(result[1], true);

            result = sandbox.Exec(new bool[] { true, false }, new bool[] { false, true });
            Assert.AreEqual(result[0], true);
            Assert.AreEqual(result[1], false);
        }

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
