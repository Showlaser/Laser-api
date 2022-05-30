using LaserAPI.Enums;
using System.Collections.Generic;

namespace LaserAPI.Models.FromFrontend.LasershowGenerator
{
    public class SongData
    {
        public List<string> Genres { get; set; }
        public int? Bpm { get; set; }
        public string SongName { get; set; }
        public bool SaveLasershow { get; set; }
        public MusicGenre MusicGenre { get; set; }
        public bool IsPlaying { get; set; }
    }
}
