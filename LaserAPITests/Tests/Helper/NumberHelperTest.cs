using LaserAPI.Models.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace LaserAPITests.Tests.Helper
{
    [TestClass]
    public class NumberHelperTest
    {
        [TestMethod]
        public void DoubleIsBetweenOrEqualToTest()
        {
            Assert.IsTrue(0.5.IsBetweenOrEqualTo(0.1, 1));
        }

        [TestMethod]
        public void DoubleIsBetweenOrEqualToLowerBoundIsInclusiveTest()
        {
            Assert.IsTrue(0.1.IsBetweenOrEqualTo(0.1, 1));
        }

        [TestMethod]
        public void DoubleIsBetweenOrEqualToUpperBoundIsInclusiveTest()
        {
            Assert.IsTrue(1.0.IsBetweenOrEqualTo(0.1, 1));
        }

        [TestMethod]
        public void DoubleIsBetweenOrEqualToNullTest()
        {
            Assert.Throws<NoNullAllowedException>(() => double.NaN.IsBetweenOrEqualTo(0.1, 1));
        }

        [TestMethod]
        public void DoubleIsBetweenOrEqualToToHighTest()
        {
            Assert.IsFalse(1.1.IsBetweenOrEqualTo(0.1, 1));
        }

        [TestMethod]
        public void DoubleIsBetweenOrEqualToToLowTest()
        {
            Assert.IsFalse(0.IsBetweenOrEqualTo(0.1, 1));
        }

        [TestMethod]
        public void IntIsBetweenOrEqualToTest()
        {
            Assert.IsTrue(2.IsBetweenOrEqualTo(0, 5));
        }

        [TestMethod]
        public void IntIsBetweenOrEqualToBoundsAreInclusiveTest()
        {
            Assert.IsTrue(0.IsBetweenOrEqualTo(0, 5));
            Assert.IsTrue(5.IsBetweenOrEqualTo(0, 5));
        }

        [TestMethod]
        public void IntIsBetweenOrEqualToToHighTest()
        {
            Assert.IsFalse(5.IsBetweenOrEqualTo(0, 1));
        }

        [TestMethod]
        public void IntIsBetweenOrEqualToToLowTest()
        {
            Assert.IsFalse(0.IsBetweenOrEqualTo(1, 4));
        }

        [TestMethod]
        public void GetLowestNumberTest()
        {
            Assert.AreEqual(-4000, NumberHelper.GetLowestNumber(-4000, 4000));
        }

        [TestMethod]
        public void GetLowestNumberReverseOrderTest()
        {
            Assert.AreEqual(-4000, NumberHelper.GetLowestNumber(4000, -4000));
        }

        [TestMethod]
        public void GetHighestNumberTest()
        {
            Assert.AreEqual(4000, NumberHelper.GetHighestNumber(-4000, 4000));
        }

        [TestMethod]
        public void GetHighestNumberReverseOrderTest()
        {
            Assert.AreEqual(4000, NumberHelper.GetHighestNumber(4000, -4000));
        }

        [TestMethod]
        public void GetDifferenceBetweenTwoNumbersTest()
        {
            Assert.AreEqual(8000, NumberHelper.GetDifferenceBetweenTwoNumbers(-4000, 4000));
        }

        [TestMethod]
        public void GetDifferenceBetweenTwoNumbersReverseOrderTest()
        {
            Assert.AreEqual(8000, NumberHelper.GetDifferenceBetweenTwoNumbers(4000, -4000));
        }

        [TestMethod]
        public void MapScalesValueIntoNewRangeTest()
        {
            Assert.AreEqual(50, 5.Map(0, 10, 0, 100));
        }

        [TestMethod]
        public void MapHandlesLowerBoundTest()
        {
            Assert.AreEqual(0, 0.Map(0, 10, 0, 100));
        }

        [TestMethod]
        public void MapHandlesUpperBoundTest()
        {
            Assert.AreEqual(100, 10.Map(0, 10, 0, 100));
        }
    }
}
