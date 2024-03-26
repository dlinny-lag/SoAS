using System;
using System.Collections.Generic;
using System.Linq;
using AAF.Services.Differences;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Services.UnitTests
{
    [TestClass]
    public class DifferencesTest
    {
        private static readonly IList<Type> declaredComparers;

        static DifferencesTest()
        {
            var rootType = typeof(IElementComparer); // load assembly
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            declaredComparers = assemblies.SelectMany(asm =>
                asm.GetTypes().Where(t => rootType.IsAssignableFrom(t))).ToArray();
        }

        static bool IsPowerOfTwo(ulong x)
        {
            return (x & (x - 1)) == 0;
        }

        [TestMethod]
        public void AreDeclarationsConsistent()
        {
            var validTypes = declaredComparers.Where(t => !t.IsAbstract).ToArray();
            // String, Race, Animation, AnimationGroup, Position, PositionTree, PositionItem, AnimationCompatibility
            Assert.AreEqual(8, validTypes.Length); 

            foreach (Type comparerType in validTypes)
            {
                Assert.IsTrue(comparerType.IsSealed);

                Type comparerInterface = comparerType.GetInterfaces().Single(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IElementComparer<,>));

                Type diffEnumType = comparerInterface.GetGenericArguments()[1];
                Assert.IsTrue(diffEnumType.GetCustomAttributes(typeof(FlagsAttribute), false).Any(), $"{diffEnumType.FullName} must be flags enum");
                Assert.IsTrue(Enum.IsDefined(diffEnumType, 0), $"{diffEnumType.FullName} must contain definition of None");
                var values = Enum.GetValues(diffEnumType);
                Assert.IsTrue(values.Length > 1);
                foreach (object value in values)
                {
                    ulong num = Convert.ToUInt64(value);
                    Assert.IsTrue(IsPowerOfTwo(num));
                }
            }

            
        }
    }
}