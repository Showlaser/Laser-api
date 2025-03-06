using System;
using System.Collections.Generic;

namespace LaserAPI.Models.ToFrontend.LasershowSpotifyConnector
{
    public class LasershowSpotifyConnectorViewmodel
    {
        public Guid Uuid { get; set; }
        public List<LasershowSpotifyConnectorSongViewmodel> SpotifySongs { get; set; }
        public Guid LasershowUuid { get; set; }
    }
}
