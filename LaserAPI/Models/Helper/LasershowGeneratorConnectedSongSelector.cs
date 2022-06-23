using System.Threading.Tasks;
using LaserAPI.Logic;
using LaserAPI.Models.FromFrontend.LasershowGenerator;

namespace LaserAPI.Models.Helper
{
    public class LasershowGeneratorConnectedSongSelector
    {
        private readonly LasershowSpotifyConnectorLogic _lasershowSpotifyConnectorLogic;

        public LasershowGeneratorConnectedSongSelector(LasershowSpotifyConnectorLogic lasershowSpotifyConnectorLogic)
        {
            _lasershowSpotifyConnectorLogic = lasershowSpotifyConnectorLogic;
        }

        public async Task Select(SongData songData)
        {
            var result = await _lasershowSpotifyConnectorLogic.Find(songData.SongId);
            result[0].SpotifySongs[0].SpotifySongId

            songData.
        }
    }
}
