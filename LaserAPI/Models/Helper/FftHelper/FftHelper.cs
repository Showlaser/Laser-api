using System;

namespace LaserAPI.Models.Helper.FftHelper
{
    public static class FftHelper
    {
        public static AlgorithmSettings GetAlgorithmSettingsByGenre(Enums.MusicGenre genre)
        {
            return genre.ToString().ToLower() switch
            {
                "hardstyle" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.008 },
                "hardcore" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.008 },
                "classic" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "techno" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "metal" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "trance" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "rock" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "house" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                _ => null
            };
        }
    }
}
