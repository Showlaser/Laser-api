using System.Collections.Generic;
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
        public void DoubleIsBetweenOrEqualToNullTest()
        {
            Assert.ThrowsException<NoNullAllowedException>(() => double.NaN.IsBetweenOrEqualTo(0.1, 1));
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
            int lowestNumber = NumberHelper.GetLowestNumber(-4000, 4000);
            Assert.IsTrue(lowestNumber < 4000);
        }

        [TestMethod]
        public void GetLowestNumberReverseOrderTest()
        {
            int lowestNumberReverseOrder = NumberHelper.GetLowestNumber(-4000, 4000);
            Assert.IsTrue(lowestNumberReverseOrder < 4000);
        }

        [TestMethod]
        public void GetHighestNumberTest()
        {
            int highestNumber = NumberHelper.GetHighestNumber(-4000, 4000);
            Assert.IsTrue(highestNumber > -4000);
        }

        [TestMethod]
        public void GetHighestNumberReverseOrderTest()
        {
            int highestNumberReverseOrder = NumberHelper.GetHighestNumber(-4000, 4000);
            Assert.IsTrue(highestNumberReverseOrder > -4000);
        }

        [TestMethod]
        public void GetDifferenceBetweenTwoNumbersTest()
        {
            List<int> numbers = new() { -400, 400, 800, -4000, 4000, 8000, 4000, 4000, 0, 0, 0, 0 }; 
            // first and second number are the numbers to check difference the third number is the difference between the two numbers
            for (int i = 0; i < 12; i+=3)
            {
                int number1 = numbers[i];
                int number2 = numbers[i + 1];
                int answer = numbers[i + 2];
                int difference = NumberHelper.CalculateDifference(number1, number2);
                Assert.IsTrue(difference == answer);
            }
        }
    }
}
