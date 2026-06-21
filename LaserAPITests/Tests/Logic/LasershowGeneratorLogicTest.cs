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
            Dictionary<string, MusicGenre> genreBySpotifyName = new()
            {
                { "house", MusicGenre.House },
                { "Hardstyle", MusicGenre.Hardstyle },
                { "Hardcore", MusicGenre.Hardcore },
                { "CLASSIC", MusicGenre.Classic }
            };

            foreach ((string spotifyName, MusicGenre expected) in genreBySpotifyName)
            {
                MusicGenre value = LaserShowGeneratorAlgorithm.GetMusicGenreFromSpotifyGenre([spotifyName]);
                Assert.AreEqual(expected, value);
            }
        }

        [TestMethod]
        public void GetMusicGenreFromUnknownSpotifyGenreReturnsUnsupportedTest()
        {
            MusicGenre value = LaserShowGeneratorAlgorithm.GetMusicGenreFromSpotifyGenre(["some-unknown-genre"]);
            Assert.AreEqual(MusicGenre.Unsupported, value);
        }

        [TestMethod]
        public void GetMusicGenreFromEmptyListReturnsUnsupportedTest()
        {
            MusicGenre value = LaserShowGeneratorAlgorithm.GetMusicGenreFromSpotifyGenre([]);
            Assert.AreEqual(MusicGenre.Unsupported, value);
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
