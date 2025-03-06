using LaserAPI.Enums;
using LaserAPI.Logic;
using LaserAPI.Models.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class LasershowGeneratorLogicTest
    {
        [TestMethod]
        public void GetMusicGenreFromSpotifyGenreTest()
        {
            List<string> genres = new()
            {
                "house",
                "Hardstyle",
                "Hardcore",
                "CLASSIC"
            };

            foreach (string? genre in genres)
            {
                MusicGenre value = LaserShowGeneratorAlgorithm.GetMusicGenreFromSpotifyGenre(new List<string> { genre });
                Assert.IsNotNull(value);
                Assert.IsTrue(value.ToString().Length > 2);
            }
        }

        [TestMethod]
        public void GetFftFrequencyRangeByGenreTest()
        {
            AlgorithmSettings settings = LaserShowGeneratorAlgorithm.GetAlgorithmSettingsByGenre(MusicGenre.Hardcore);
            Assert.IsNotNull(settings);
            Assert.IsTrue(settings.FrequencyRange.Start.Value == 2);
            Assert.IsTrue(settings.FrequencyRange.End.Value == 3);
        }
    }
}
