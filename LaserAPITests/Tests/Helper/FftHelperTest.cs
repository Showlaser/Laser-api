using LaserAPI.Enums;
using LaserAPI.Models.Helper.FftHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LaserAPITests.Tests.Helper
{
    [TestClass]
    public class FftHelperTest
    {
        [TestMethod]
        public void GetFftFrequencyRangeByGenreTest()
        {
            Range range = FftHelper.GetFftFrequencyRangeByGenre(MusicGenre.Hardcore);
            Assert.IsNotNull(range);
            Assert.IsTrue(range.Start.Value == 0);
            Assert.IsTrue(range.End.Value == 5);
        }
    }
}
