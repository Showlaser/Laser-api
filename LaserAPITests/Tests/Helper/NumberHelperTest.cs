using LaserAPI.Models.Helper;
using NUnit.Framework;
using System.Data;

namespace LaserAPITests.Tests.Helper
{
    [TestFixture]
    internal class NumberHelperTest
    {
        [Test]
        public void DoubleIsBetweenOrEqualToTest()
        {
            Assert.True(0.5.IsBetweenOrEqualTo(0.1, 1));
        }

        [Test]
        public void DoubleIsBetweenOrEqualToNullTest()
        {
            Assert.Throws<NoNullAllowedException>(() => double.NaN.IsBetweenOrEqualTo(0.1, 1));
        }

        [Test]
        public void DoubleIsBetweenOrEqualToToHighTest()
        {
            Assert.IsFalse(1.1.IsBetweenOrEqualTo(0.1, 1));
        }

        [Test]
        public void DoubleIsBetweenOrEqualToToLowTest()
        {
            Assert.IsFalse(0.IsBetweenOrEqualTo(0.1, 1));
        }

        [Test]
        public void IntIsBetweenOrEqualToTest()
        {
            Assert.True(2.IsBetweenOrEqualTo(0, 5));
        }

        [Test]
        public void IntIsBetweenOrEqualToToHighTest()
        {
            Assert.IsFalse(5.IsBetweenOrEqualTo(0, 1));
        }

        [Test]
        public void IntIsBetweenOrEqualToToLowTest()
        {
            Assert.IsFalse(0.IsBetweenOrEqualTo(1, 4));
        }

        [Test]
        public void GetLowestNumberTest()
        {
            int lowestNumber = NumberHelper.GetLowestNumber(-4000, 4000);
            Assert.IsTrue(lowestNumber < 4000);
        }

        [Test]
        public void GetLowestNumberReverseOrderTest()
        {
            int lowestNumberReverseOrder = NumberHelper.GetLowestNumber(-4000, 4000);
            Assert.IsTrue(lowestNumberReverseOrder < 4000);
        }

        [Test]
        public void GetHighestNumberTest()
        {
            int highestNumber = NumberHelper.GetHighestNumber(-4000, 4000);
            Assert.IsTrue(highestNumber > -4000);
        }

        [Test]
        public void GetHighestNumberReverseOrderTest()
        {
            int highestNumberReverseOrder = NumberHelper.GetHighestNumber(-4000, 4000);
            Assert.IsTrue(highestNumberReverseOrder > -4000);
        }
    }
}
