using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Reflection;
using Model;


namespace UniversalAssetCreator.TestApp
{
    [TestClass]
    public class VisualAssetEnumTests
    {
        [TestMethod]
        public void TestReflectionOutput()
        {
            var attributeValue = EnumTools.GetAttribute<ImageDataAttribute>(VisualAssets.BadgeLogoScale100);
            //[ImageData(24, 24, 22, 23, PlacmentType.TopWidth1PxMargin)] BadgeLogoScale100,

            Assert.IsNotNull(attributeValue);
            Assert.AreEqual(attributeValue.Width, 24);
            Assert.AreEqual(attributeValue.Height,24);
            Assert.AreEqual(attributeValue.HowToPlace, PlacmentType.TopWidth1PxMargin);
            Assert.AreEqual(attributeValue.IconicHeight,23);
            Assert.AreEqual(attributeValue.IconicWidth,22);

        }
    }
}
