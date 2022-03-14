using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Helper.FftHelper
{
    public static class FftHelper
    {
        public static Range GetFftFrequencyRangeByGenre(Enums.MusicGenre genre)
        {
            Dictionary<Enums.MusicGenre, Range> fftFrequencyRange = new();
            fftFrequencyRange.Add(Enums.MusicGenre.Hardstyle, new Range(1, 5));
            fftFrequencyRange.Add(Enums.MusicGenre.Hardcore, new Range(1, 5));
            fftFrequencyRange.Add(Enums.MusicGenre.Classic, new Range(5, 10));
            fftFrequencyRange.Add(Enums.MusicGenre.Techno, new Range(1, 5));
            fftFrequencyRange.Add(Enums.MusicGenre.Metal, new Range(3, 8));
            fftFrequencyRange.Add(Enums.MusicGenre.Trance, new Range(1, 5));
            fftFrequencyRange.Add(Enums.MusicGenre.Rock, new Range(3, 8));
            fftFrequencyRange.Add(Enums.MusicGenre.House, new Range(1, 5));

            return fftFrequencyRange[genre];
        }
    }
}
