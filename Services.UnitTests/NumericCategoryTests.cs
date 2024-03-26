using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SceneServices.TagCategories;

namespace Services.UnitTests
{
    [TestClass]
    public class NumericCategoryTests
    {
        [TestMethod]
        public void Love9()
        {
            var value = "Love9".AsNumeric();
            Assert.AreEqual(NumericType.Love, value.Type);
            Assert.AreEqual(9, value.Value);
        }

        [TestMethod]
        public void Love_9()
        {
            var value = "Love-9".AsNumeric();
            Assert.AreEqual(NumericType.Love, value.Type);
            Assert.AreEqual(-9, value.Value);
        }

        [TestMethod]
        public void Love0()
        {
            var value = "Love0".AsNumeric();
            Assert.AreEqual(NumericType.Love, value.Type);
            Assert.AreEqual(0, value.Value);
        }

        [TestMethod]
        public void Stim_9()
        {
            var value = "Stim-9".AsNumeric();
            Assert.AreEqual(NumericType.Stim, value.Type);
            Assert.AreEqual(-9, value.Value);
        }

        [TestMethod]
        public void Dom_9()
        {
            var value = "Dom-9".AsNumeric();
            Assert.AreEqual(NumericType.Dom, value.Type);
            Assert.AreEqual(-9, value.Value);
        }

        [TestMethod]
        public void Held_9()
        {
            var value = "Held-9".AsNumeric();
            Assert.AreEqual(NumericType.Held, value.Type);
            Assert.AreEqual(-9, value.Value);
        }

        [TestMethod]
        public void Fails()
        {
            var value = "Love".AsNumeric();
            Assert.AreEqual(NumericType.None, value.Type);
            Assert.AreEqual(0, value.Value);

            value = "LoveHeld".AsNumeric();
            Assert.AreEqual(NumericType.None, value.Type);
            Assert.AreEqual(0, value.Value);

            value = "LoveHeld5".AsNumeric();
            Assert.AreEqual(NumericType.None, value.Type);
            Assert.AreEqual(0, value.Value);

            value = "HeldLove5".AsNumeric();
            Assert.AreEqual(NumericType.None, value.Type);
            Assert.AreEqual(0, value.Value);

            value = "LoveHeld-5".AsNumeric();
            Assert.AreEqual(NumericType.None, value.Type);
            Assert.AreEqual(0, value.Value);

            value = "Love0Held-5".AsNumeric();
            Assert.AreEqual(NumericType.None, value.Type);
            Assert.AreEqual(0, value.Value);
        }

        [TestMethod]
        public void TestScale()
        {
            Assert.AreEqual(0, 0.NormalizeNumeric());

            Assert.AreEqual(-10, -1.NormalizeNumeric());
            Assert.AreEqual(10, 1.NormalizeNumeric());

            Assert.AreEqual(-30, -3.NormalizeNumeric());
            Assert.AreEqual(30, 3.NormalizeNumeric());

            Assert.AreEqual(-50, -5.NormalizeNumeric());
            Assert.AreEqual(50, 5.NormalizeNumeric());

            Assert.AreEqual(-70, -7.NormalizeNumeric());
            Assert.AreEqual(70, 7.NormalizeNumeric());

            Assert.AreEqual(-100, -9.NormalizeNumeric());
            Assert.AreEqual(100, 9.NormalizeNumeric());

            Assert.AreEqual(-100, -15.NormalizeNumeric());
            Assert.AreEqual(100, 25.NormalizeNumeric());
        }
    }
}
