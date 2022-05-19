using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.LasershowSpotify;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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
            bool connectorExists = await _lasershowSpotifyConnectorDal.Exists(connector.LasershowUuid);
            if (string.IsNullOrEmpty(connector.SpotifySongId) && connectorExists)
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
    }
}
