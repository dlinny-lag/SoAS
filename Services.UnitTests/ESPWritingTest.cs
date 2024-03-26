using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Utils;

namespace Services.UnitTests
{
    [TestClass]
    public class ESPWritingTest
    {
        private static readonly byte[] empty8_esp;
        private static readonly byte[] empty9_esp;

        static byte[] Read(string name)
        {
            using (var stream = typeof(ESPWritingTest).Assembly.GetManifestResourceStream($"{typeof(ESPWritingTest).Assembly.GetName().Name}.{name}"))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        static ESPWritingTest()
        {
            empty8_esp = Read("empty8.esp");
            empty9_esp = Read("empty9.esp");
        }
        
        [TestMethod]
        public void Write8()
        {
            const string creator = "CREATOR";
            Assert.AreEqual(8, creator.Length + 1);
            byte[] content = Fallout4ModsHelper.GenerateEmptyESL(creator, "SUMMARY");
            CollectionAssert.AreEqual(empty8_esp, content);
        }
        [TestMethod]
        public void Write9()
        {
            const string creator = "CREATOR9";
            Assert.AreEqual(9, creator.Length + 1);
            byte[] content = Fallout4ModsHelper.GenerateEmptyESL(creator, "SUMMARY");
            CollectionAssert.AreEqual(empty9_esp, content);
        }

        [TestMethod]
        public void CheckDataLength()
        {
            byte[] content = Fallout4ModsHelper.GenerateEmptyESL("Dlinny_Lag", "Package for Animation Scenes");
            Assert.AreEqual(0x50, content[4]);
        }
    }

}
