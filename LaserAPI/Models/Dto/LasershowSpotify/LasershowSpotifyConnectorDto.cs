using System;

namespace LaserAPI.Models.Dto.LasershowSpotify
{
    public class LasershowSpotifyConnectorDto
    {
        public Guid Uuid { get; set; }
        public string SpotifySongId { get; set; }
        public Guid LasershowUuid { get; set; }
    }
}
