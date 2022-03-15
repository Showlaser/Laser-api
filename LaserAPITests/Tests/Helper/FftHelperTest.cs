using LaserAPI.Enums;
using LaserAPI.Models.Helper.FftHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LaserAPITests.Tests.Helper
{
    [TestClass]
    public class FftHelperTest
    {
        [TestMethod]
        public void GetFftFrequencyRangeByGenreTest()
        {
            AlgorithmSettings settings = FftHelper.GetAlgorithmSettingsByGenre(MusicGenre.Hardcore);
            Assert.IsNotNull(settings);
            Assert.IsTrue(settings.FrequencyRange.Start.Value == 2);
            Assert.IsTrue(settings.FrequencyRange.End.Value == 3);
        }
    }
}
