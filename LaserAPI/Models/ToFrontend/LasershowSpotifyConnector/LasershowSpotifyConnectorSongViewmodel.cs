using System;

namespace LaserAPI.Models.ToFrontend.LasershowSpotifyConnector
{
    public class LasershowSpotifyConnectorSongViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid LasershowSpotifyConnectorUuid { get; set; }
        public string SpotifySongId { get; set; }
    }
}
