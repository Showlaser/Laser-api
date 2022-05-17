using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.LasershowSpotify;

namespace LaserAPI.Logic
{
    public class LasershowSpotifyConnectorLogic
    {
        private readonly ILasershowSpotifyConnectorDal _lasershowSpotifyConnectorDal;

        public LasershowSpotifyConnectorLogic(ILasershowSpotifyConnectorDal lasershowSpotifyConnectorDal)
        {
            _lasershowSpotifyConnectorDal = lasershowSpotifyConnectorDal;
        }

        public async Task Add(LasershowSpotifyConnectorDto connector)
        {
            bool connectorExists = await _lasershowSpotifyConnectorDal.Exists(connector.LasershowUuid);
            if (connectorExists)
            {
                throw new DuplicateNameException();
            }

            await _lasershowSpotifyConnectorDal.Add(connector);
        }

        public async Task<List<LasershowSpotifyConnectorDto>> All()
        {
            return await _lasershowSpotifyConnectorDal.All();
        }

        public async Task Remove(Guid lasershowUuid)
        {
            await _lasershowSpotifyConnectorDal.Remove(lasershowUuid);
        }
    }
}
