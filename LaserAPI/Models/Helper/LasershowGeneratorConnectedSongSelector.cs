using LaserAPI.Logic;
using LaserAPI.Models.Dto.LasershowSpotify;
using LaserAPI.Models.FromFrontend.LasershowGenerator;
using System.Threading.Tasks;

namespace LaserAPI.Models.Helper
{
    public class LasershowGeneratorConnectedSongSelector
    {
        private readonly bool _algorithmIsRunning;

        public void Start(SongData songData, string deviceName)
        {
            LaserShowGeneratorAlgorithm.SetSongData(songData);
            LaserShowGeneratorAlgorithm.Start(deviceName);
        }

        public void Stop()
        {
            LaserShowGeneratorAlgorithm.Stop();
        }

        public async Task Select(SongData songData, LasershowSpotifyConnectorLogic lasershowSpotifyConnectorLogic)
        {
            LasershowSpotifyConnectorDto existingSpotifyConnector = await lasershowSpotifyConnectorLogic.Find(songData.SongId);
            if (existingSpotifyConnector == null)
            {
                await GenerateLiveLaserShow();
            }
            else
            {
                await PlayConnectedSong();
            }
        }

        public async Task GenerateLiveLaserShow()
        {
        }

        public async Task PlayConnectedSong()
        {

        }
    }
}
