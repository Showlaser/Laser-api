using LaserAPI.Enums;
using LaserAPI.Models.Helper.MusicGenre;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LaserAPITests.Tests.Helper
{
    [TestClass]
    public class MusicGenreHelperTest
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
                MusicGenre value = MusicGenreHelper.GetMusicGenreFromSpotifyGenre(new List<string> { genre });
                Assert.IsNotNull(value);
                Assert.IsTrue(value.ToString().Length > 2);
            }
        }
    }
}
