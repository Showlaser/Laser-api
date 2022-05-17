using System;

namespace LaserAPI.Models.ToFrontend.LasershowSpotifyConnector
{
    public class LasershowSpotifyConnectorViewmodel
    {
        public Guid Uuid { get; set; }
        public string SpotifySongId { get; set; }
        public Guid LasershowUuid { get; set; }
    }
}
