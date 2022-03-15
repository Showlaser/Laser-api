using LaserAPI.Enums;
using System.Collections.Generic;

namespace LaserAPI.Models.FromFrontend.LasershowGenerator
{
    public class SongData
    {
        public int? ProgressMs { get; set; }
        public List<string> Genres { get; set; }
        public int? Bpm { get; set; }
        public MusicGenre MusicGenre { get; set; }

    }
}
