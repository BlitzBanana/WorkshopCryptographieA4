using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cryptography;

namespace Cryptography_Tests
{
    [TestClass]
    public class ToolsTests
    {
        [TestMethod]
        public void CircularLeftShift()
        {
            var boolArray = new bool[]
            {
                true,
                false,
                false,
                true
            };

            var shifted = Tools.BoolArrayCircularLeftShift(boolArray, 1);

            Assert.AreEqual(shifted[0], false);
            Assert.AreEqual(shifted[1], false);
            Assert.AreEqual(shifted[2], true);
            Assert.AreEqual(shifted[3], true);

            shifted = Tools.BoolArrayCircularLeftShift(boolArray, 2);

            Assert.AreEqual(shifted[0], false);
            Assert.AreEqual(shifted[1], true);
            Assert.AreEqual(shifted[2], true);
            Assert.AreEqual(shifted[3], false);

            shifted = Tools.BoolArrayCircularLeftShift(boolArray, 3);

            Assert.AreEqual(shifted[0], true);
            Assert.AreEqual(shifted[1], true);
            Assert.AreEqual(shifted[2], false);
            Assert.AreEqual(shifted[3], false);
        }

        [TestMethod]
        public void CharBitsTests()
        {
            var boolArray = Tools.CharToBoolArray('a');
            var c = Tools.BoolArrayToChar(boolArray);
            Assert.AreEqual(c, 'a');
        }
    }
}
