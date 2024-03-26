using Microsoft.VisualStudio.TestTools.UnitTesting;
using SceneServices.TagCategories;

namespace Services.UnitTests
{
    [TestClass]
    public class CategorizerTests
    {
        [TestMethod]
        public void ContactOK()
        {
            Assert.AreEqual(TagCategoryTypes.Contact, "PENISTONIPPLE".GetCategory());
        }

        [TestMethod]
        public void ActorTypeOK()
        {
            Assert.AreEqual(TagCategoryTypes.ActorTypes, "F".GetCategory());
            Assert.AreEqual(TagCategoryTypes.ActorTypes, "M".GetCategory());
            Assert.AreEqual(TagCategoryTypes.ActorTypes, "F_F".GetCategory());
            Assert.AreEqual(TagCategoryTypes.ActorTypes, "M_F".GetCategory());
            Assert.AreEqual(TagCategoryTypes.ActorTypes, "3P".GetCategory());
            Assert.AreEqual(TagCategoryTypes.ActorTypes, "F_FGHOULM_FGHOULM".GetCategory());
            Assert.AreEqual(TagCategoryTypes.ActorTypes, "F_SMUTANTM_SMUTANTM_SMUTANTM".GetCategory());

            
        }

        [TestMethod]
        public void FeralGhoul()
        {
            // FGHOULM tag looks like a generic indicator of animation with feral ghoul
            Assert.AreEqual(TagCategoryTypes.Unknown, "FGHOULM".GetCategory()); 
        }
    }
}