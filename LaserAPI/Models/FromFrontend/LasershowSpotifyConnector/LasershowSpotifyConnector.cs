using System;

namespace LaserAPI.Models.FromFrontend.LasershowSpotifyConnector
{
    public class LasershowSpotifyConnector
    {
        public Guid Uuid { get; set; }
        public string SpotifySongId { get; set; }
        public Guid LasershowUuid { get; set; }
    }
}
