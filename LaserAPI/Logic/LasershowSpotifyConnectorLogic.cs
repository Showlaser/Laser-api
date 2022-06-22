using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.LasershowSpotify;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LasershowSpotifyConnectorLogic
    {
        private readonly ILasershowSpotifyConnectorDal _lasershowSpotifyConnectorDal;

        public LasershowSpotifyConnectorLogic(ILasershowSpotifyConnectorDal lasershowSpotifyConnectorDal)
        {
            _lasershowSpotifyConnectorDal = lasershowSpotifyConnectorDal;
        }

        public async Task AddOrUpdate(LasershowSpotifyConnectorDto connector)
        {
            List<string> songs = connector.SpotifySongs.Select(ss => ss.SpotifySongId).ToList();
            bool songIsAlreadyConnectedToADifferentLasershow =
                await _lasershowSpotifyConnectorDal.SongsExistsInAnotherLasershow(songs, connector.LasershowUuid);

            if (songIsAlreadyConnectedToADifferentLasershow)
            {
                throw new DuplicateNameException();
            }

            bool connectorExists = await _lasershowSpotifyConnectorDal.Exists(connector.LasershowUuid);
            if (!connector.SpotifySongs.Any() && connectorExists)
            {
                await _lasershowSpotifyConnectorDal.Remove(connector.LasershowUuid);
                return;
            }

            if (connectorExists)
            {
                await _lasershowSpotifyConnectorDal.Remove(connector.LasershowUuid);
            }

            await _lasershowSpotifyConnectorDal.Add(connector);
        }

        public async Task<List<LasershowSpotifyConnectorDto>> All()
        {
            return await _lasershowSpotifyConnectorDal.All();
        }

        public async Task<LasershowSpotifyConnectorDto> Find(string spotifyId)
        {
            return await _lasershowSpotifyConnectorDal.Find(spotifyId);
        }
    }
}
