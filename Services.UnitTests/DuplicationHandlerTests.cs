using AAF.Services.AAFImport;
using AAFModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Services.UnitTests
{
    [TestClass]
    public class DuplicationHandlerTests
    {
        [TestMethod]
        public void EnumeratorTest()
        {
            string id = "test";
            Animation reference = new Animation("aaa.xml", 0){Id = id};
            DuplicationsHolder<Animation> holder = new DuplicationsHolder<Animation>(id, reference);
            Animation dup1 = new Animation("bbb.xml", 0){Id = id};
            holder.AddDuplicates(dup1);

            int count = 0;
            foreach (Animation animation in holder)
            {
                count++;
            }

            Assert.AreEqual(holder.Count, count);
        }
    }
}
