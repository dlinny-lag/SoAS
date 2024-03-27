using System;
using System.IO;
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

#if DEBUG
        [TestMethod]
        public void GenerateCreaturesList()
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SoASDocs");
            Directory.CreateDirectory(folder);
            foreach (Creature creature in CreaturesFactory.Supported)
            {
                string skeleton = creature.Skeleton;
                string bodyParts = creature.ToString();
                File.WriteAllText(Path.Combine(folder, $"{skeleton}.txt"), bodyParts);
            }
        }
#endif
    }
}