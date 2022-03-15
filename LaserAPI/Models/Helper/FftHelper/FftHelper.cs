using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Helper.FftHelper
{
    public static class FftHelper
    {
        private static readonly Dictionary<Enums.MusicGenre, AlgorithmSettings> AlgorithmSettings = new()
        {
            { Enums.MusicGenre.Hardstyle, new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.015 } },
            { Enums.MusicGenre.Hardcore, new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.015 } },
            { Enums.MusicGenre.Classic, new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 } },
            { Enums.MusicGenre.Techno, new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 } },
            { Enums.MusicGenre.Metal, new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 } },
            { Enums.MusicGenre.Trance, new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 } },
            { Enums.MusicGenre.Rock, new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 } },
            { Enums.MusicGenre.House, new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 } },
        };

        public static AlgorithmSettings GetAlgorithmSettingsByGenre(Enums.MusicGenre genre)
        {
            return AlgorithmSettings[genre];
        }
    }
}
