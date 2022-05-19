using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Dto.LasershowSpotify
{
    public class LasershowSpotifyConnectorDto
    {
        public Guid Uuid { get; set; }
        public List<LasershowSpotifyConnectorSongDto> SpotifySongs { get; set; }
        public Guid LasershowUuid { get; set; }
    }
}
