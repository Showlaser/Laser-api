using System;

namespace LaserAPI.Models.Dto.LasershowSpotify
{
    public class LasershowSpotifyConnectorSongDto
    {
        public Guid Uuid { get; set; }
        public Guid LasershowSpotifyConnectorUuid { get; set; }
        public string SpotifySongId { get; set; }
    }
}
