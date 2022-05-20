using System;

namespace LaserAPI.Models.FromFrontend.LasershowSpotifyConnector
{
    public class LasershowSpotifyConnectorSong
    {
        public Guid Uuid { get; set; }
        public Guid LasershowSpotifyConnectorUuid { get; set; }
        public string SpotifySongId { get; set; }
    }
}
