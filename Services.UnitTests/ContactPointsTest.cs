using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SceneModel.ContactAreas;
using Shared.Utils;
using Single = SceneModel.ContactAreas.Single;

namespace Services.UnitTests
{
    [TestClass]
    public class ContactPointsTest
    {
        private const int ClassesCount = 52 + 1; // FakeRoot for UI
        private const string Any = "Any"; // name for the property with all zero values
        private static readonly IList<Type> declaredContactAreas;

        private const string SymmetryTypeFieldName = nameof(Locationness<SingleNoSymmetry>.Type);
        private const string SymmetryTypeFieldName1 = nameof(Locationness<SingleNoSymmetry, SingleNoSymmetry>.Type1);
        private const string SymmetryTypeFieldName2 = nameof(Locationness<SingleNoSymmetry, SingleNoSymmetry>.Type2);

        static ContactPointsTest()
        {
            var rootType = typeof(ContactArea); // load assembly
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            declaredContactAreas = assemblies.SelectMany(asm =>
                asm.GetTypes().Where(t => rootType.IsAssignableFrom(t))).ToArray();
        }

        private static readonly Type[] allowedSymmetryEnums = new[]
        {
            typeof(SingleNoSymmetry),
            typeof(LeftRightSymmetry),
            typeof(TopBottomSymmetry),
            typeof(FrontBackSymmetry),
            typeof(Linear3Asymmetry),
            typeof(Azimuthal3Asymmetry)
        };

        [TestMethod]
        public void ClassesStructureOK()
        {
            var validTypes = declaredContactAreas.Where(t => !t.IsAbstract).ToArray();
            Assert.AreEqual(ClassesCount, validTypes.Length);

            foreach (Type contactPointsType in validTypes)
            {
                Assert.IsTrue(contactPointsType.IsSealed, $"{contactPointsType.Name} must be sealed");
                Assert.AreEqual(0, contactPointsType.GetConstructors(BindingFlags.Public).Length, $"{contactPointsType.Name} must not have public constructors");

                Assert.IsTrue(contactPointsType.ExtractGenericArguments(typeof(ContactArea<>), out var locationnessArgs));
                
                Type locationnessType = locationnessArgs[0];
                Type[] symmetryTypes = GetSymmetryTypes(locationnessType);
                TestProperties(contactPointsType, symmetryTypes);
            }
        }
        private static Type[] GetSymmetryTypes(Type locationnessType)
        {
            bool isSingleSymmetry = locationnessType.ExtractGenericArguments(typeof(Locationness<>), out var singleSymmetryEnumArgs);
            bool isDoubleSymmetry = locationnessType.ExtractGenericArguments( typeof(Locationness<,>), out var doubleSymmetryEnumArgs);
            Assert.IsTrue(isSingleSymmetry || isDoubleSymmetry, $"{locationnessType.Name}");

            Type[] symmetryEnumArgs = singleSymmetryEnumArgs ?? doubleSymmetryEnumArgs;

            for (int i = 0; i < symmetryEnumArgs.Length; i++)
            {
                Assert.IsTrue(symmetryEnumArgs[i].IsIn(allowedSymmetryEnums), $"{symmetryEnumArgs[i]}");
            }

            return symmetryEnumArgs;
        }

        private static void TestProperties(Type contactPointsType, Type[] symmetryTypes)
        {
            Assert.IsTrue(symmetryTypes.Length == 1 || symmetryTypes.Length == 2);
            var fieldAny = GetField(contactPointsType, Any);
            Assert.AreEqual(true, ((ContactArea)fieldAny.GetValue(null)).IsAny);

            string[][] symmetryNamesArray = new string[symmetryTypes.Length][];
            string[] noneNameArray = new string[symmetryTypes.Length];
            for (int i = 0; i < symmetryTypes.Length; i++)
            {
                symmetryNamesArray[i] = Enum.GetNames(symmetryTypes[i]);
                noneNameArray[i] = Enum.GetName(symmetryTypes[i], 0); // should be 'None'
            }

            bool multiSymmetry = symmetryTypes.Length > 1;
            for (int i = 0; i < symmetryTypes.Length; i++)
            {
                string symmetryTypeFieldName;
                if (i == 0)
                    symmetryTypeFieldName = multiSymmetry ? SymmetryTypeFieldName1 : SymmetryTypeFieldName;
                else
                    symmetryTypeFieldName = SymmetryTypeFieldName2;

                TestOneSymmetry(contactPointsType, symmetryNamesArray[i], noneNameArray[i], symmetryTypeFieldName);
            }

            if (symmetryTypes.Length == 2)
                TestSymmetryCombinations(contactPointsType, symmetryNamesArray[0], noneNameArray[0], symmetryNamesArray[1], noneNameArray[1]);
            
        }

        private static void TestOneSymmetry(Type contactPointsType, string[] symmetryNames, string noneName, string symmetryTypeFieldName)
        {
            foreach (string symmetryName in symmetryNames)
            {
                if (symmetryName == noneName)
                    continue;
                var contactField = GetField(contactPointsType, symmetryName);
                TestFieldValue(contactField, symmetryName, symmetryTypeFieldName);
            }
        }

        private static void TestSymmetryCombinations(Type contactPointsType, 
            string[] symmetryNames1, string noname1,
            string[] symmetryNames2, string noname2)
        {
            for (int i = 0; i < symmetryNames1.Length; i++)
            {
                string firstPart = symmetryNames1[i] == noname1 ? "" : symmetryNames1[i];
                for (int j = 0; j < symmetryNames2.Length; j++)
                {
                    string secondPart = symmetryNames2[j] == noname2 ? "" : symmetryNames2[j];
                    string fieldName = $"{firstPart}{secondPart}";
                    if (string.IsNullOrEmpty(fieldName))
                        continue;
                    var contactField = GetField(contactPointsType, fieldName);
                    Assert.AreEqual(false, ((ContactArea)contactField.GetValue(null)).IsAny);
                    if (string.IsNullOrEmpty(firstPart) || string.IsNullOrEmpty(secondPart))
                        continue; // checked already
                    TestFieldValue(contactField, new []{firstPart, secondPart});
                }
            }
        }

        private static FieldInfo GetField(Type type, string fieldName)
        {
            FieldInfo f = type.GetField(fieldName, BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static);
            Assert.IsNotNull(f, $"{type.Name}.{fieldName}");
            Assert.IsTrue(f.IsInitOnly, $"{type.Name}.{fieldName}");
            return f;
        }

        private static void TestFieldValue(FieldInfo contactField, string symmetryName, string symmetryTypeFieldName)
        {
            Assert.AreEqual(false, ((ContactArea)contactField.GetValue(null)).IsAny);
            var locationessValue = contactField.GetLocationness();
            Assert.IsNotNull(locationessValue);
            TestSymmetryValue(locationessValue, symmetryName, symmetryTypeFieldName);
        }

        private static void TestSymmetryValue(Locationness locationessValue, string symmetryName, string symmetryTypeFieldName)
        {
            var symmetryField = locationessValue.GetType().GetProperty(symmetryTypeFieldName, BindingFlags.Public | BindingFlags.GetField | BindingFlags.Instance);
            Assert.IsNotNull(symmetryField);
            var symmetryEnumValue = symmetryField.GetValue(locationessValue);
            Assert.IsNotNull(symmetryEnumValue);
            var valueName = Enum.GetName(symmetryEnumValue.GetType(), symmetryEnumValue);
            Assert.AreEqual(symmetryName, valueName);
        }

        private static void TestFieldValue(FieldInfo contactField, string[] symmetryNames)
        {
            var locationessValue = contactField.GetLocationness();
            TestSymmetryValue(locationessValue, symmetryNames[0], SymmetryTypeFieldName1);
            TestSymmetryValue(locationessValue, symmetryNames[1], SymmetryTypeFieldName2);
        }
    }
    internal static class ReflectionHelper
    {
        private const string LocationnessFieldName = nameof(ContactArea<Single>.Locationness);

        public static Locationness GetLocationness(this FieldInfo contactField)
        {
            var contactPointValue = contactField.GetValue(null) as ContactArea;
            Assert.IsNotNull(contactPointValue);

            var locationnessProperty = contactPointValue.GetType().GetProperty(LocationnessFieldName, BindingFlags.Public|BindingFlags.GetField|BindingFlags.Instance);
            Assert.IsNotNull(locationnessProperty);
            var locationessValue = locationnessProperty.GetValue(contactPointValue) as Locationness ;
            Assert.IsNotNull(locationessValue);
            return locationessValue;
        }
    }

}