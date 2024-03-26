using System.Collections.Generic;
using AAF.Services.AAFImport;
using AAF.Services.Differences;
using AAFModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SceneServices;
using Shared.Utils;

namespace Services.UnitTests
{
    [TestClass]
    public class ValidationsTest
    {

        [TestMethod]
        public void IncompatibleGender()
        {
            Animation[] anims = new[]
            {
                new Animation("", 0),
                new Animation("", 1)
            };
            anims[0].Actors.Add(new Actor{Skeleton = "Human", Gender = "male"});
            anims[0].Actors.Add(new Actor{Skeleton = "Human", Gender = "female"});

            anims[1].Actors.Add(new Actor{Skeleton = "Human", Gender = "female"});
            anims[1].Actors.Add(new Actor{Skeleton = "Human", Gender = "female"});
            var comparer = new AnimationCompatibilityComparer(new Dictionary<string, Defaults>(), new Dictionary<string, Race>());
            var diff = PositionValidator.AreAnimationsCompatible(anims, comparer);

            Assert.IsTrue(diff.HasAnyOfFlags(AnimationCompatibilityDifference.ActorsGender));
        }

        [TestMethod]
        public void CompatibleGender()
        {
            Animation[] anims = new[]
            {
                new Animation("", 0),
                new Animation("", 1)
            };
            anims[0].Actors.Add(new Actor{Skeleton = "Human", Gender = "male"});
            anims[0].Actors.Add(new Actor{Skeleton = "Human", Gender = ""}); // any

            anims[1].Actors.Add(new Actor{Skeleton = "Human", Gender = ""}); // Any
            anims[1].Actors.Add(new Actor{Skeleton = "Human", Gender = "female"});

            var comparer = new AnimationCompatibilityComparer(new Dictionary<string, Defaults>(), new Dictionary<string, Race>());
            var diff = PositionValidator.AreAnimationsCompatible(anims, comparer);

            Assert.IsFalse(diff.HasAnyOfFlags(AnimationCompatibilityDifference.ActorsGender));
        }
    }
}