using System;
using System.Collections.Generic;

namespace LaserAPI.Models.FromFrontend.LasershowSpotifyConnector
{
    public class LasershowSpotifyConnector
    {
        public Guid Uuid { get; set; }
        public List<LasershowSpotifyConnectorSong> SpotifySongs { get; set; }
        public Guid LasershowUuid { get; set; }
    }
}
