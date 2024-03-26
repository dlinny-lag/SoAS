using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SceneModel.ContactAreas;
using SceneModel.Creatures;

namespace Services.UnitTests
{
    [TestClass]
    public class CreaturesTest
    {
        [TestMethod]
        public void CheckStruct()
        {
            var human = new Human();
            var feet = human.Body.Last().Children;
            Assert.AreEqual(1, feet.Length);
            string path = feet[0].ReversePath;
            Assert.AreEqual(HumanoidLeg.Any.Id, path);
            string asString = human.ToString();
        }
    }
}