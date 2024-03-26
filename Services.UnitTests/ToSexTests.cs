using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Services.UnitTests
{
    [TestClass]
    public class ToSexTests
    {
        [TestMethod]
        public void AAFToSex()
        {
            Assert.AreEqual(AAF.Services.AAFImport.Sex.Male, AAF.Services.AAFImport.AAFImportHelper.ToSex("M"));
            Assert.AreEqual(AAF.Services.AAFImport.Sex.Female, AAF.Services.AAFImport.AAFImportHelper.ToSex("F"));
            Assert.AreEqual(AAF.Services.AAFImport.Sex.Any, AAF.Services.AAFImport.AAFImportHelper.ToSex(""));
            Assert.AreEqual(AAF.Services.AAFImport.Sex.Any, AAF.Services.AAFImport.AAFImportHelper.ToSex("random"));
        }

        [TestMethod]
        public void SceneToSex()
        {
            Assert.AreEqual(SceneModel.Sex.Male, SceneServices.Scenes.AAFHelper.ToSex("M"));
            Assert.AreEqual(SceneModel.Sex.Female, SceneServices.Scenes.AAFHelper.ToSex("F"));
            Assert.AreEqual(SceneModel.Sex.Any, SceneServices.Scenes.AAFHelper.ToSex(""));
            Assert.AreEqual(SceneModel.Sex.Any, SceneServices.Scenes.AAFHelper.ToSex("random"));
        }
    }
}
