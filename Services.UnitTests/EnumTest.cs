using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Utils;

namespace Services.UnitTests
{
    [TestClass]
    public class EnumTest
    {

        [Flags]
        enum MyEnum
        {
            None = 0,
            Flag1 = 1,
            Flag2 = 2,
            Flag3 = 4,
        }

        [TestMethod]
        public void Or()
        {
            MyEnum result = MyEnum.Flag1.Or(MyEnum.Flag2);
            Assert.AreEqual(MyEnum.Flag1|MyEnum.Flag2, result);
        }

        [TestMethod]
        public void IsNone()
        {
            Assert.IsFalse(MyEnum.Flag1.IsNone());
            Assert.IsTrue(MyEnum.None.IsNone());
        }

        [TestMethod]
        public void HasFlag()
        {
            Assert.IsFalse(MyEnum.None.HasAnyOfFlags(MyEnum.Flag1));
            Assert.IsTrue(MyEnum.Flag1.HasAnyOfFlags(MyEnum.Flag1));
            Assert.IsTrue((MyEnum.Flag1 | MyEnum.Flag3).HasAnyOfFlags(MyEnum.Flag3));

            Assert.IsTrue(MyEnum.Flag2.HasAnyOfFlags(MyEnum.Flag2 | MyEnum.Flag3));
            Assert.IsTrue(MyEnum.Flag2.HasAnyOfFlags(MyEnum.Flag2, MyEnum.Flag3));

            Assert.IsFalse(MyEnum.Flag2.HasAnyOfFlags(MyEnum.Flag1 | MyEnum.Flag3));
            Assert.IsFalse(MyEnum.Flag2.HasAnyOfFlags(MyEnum.Flag1, MyEnum.Flag3));
        }
    }
}