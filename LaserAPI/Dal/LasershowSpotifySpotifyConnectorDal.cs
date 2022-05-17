using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.LasershowSpotify;
using LaserAPI.Models.Dto.Patterns;
using Microsoft.EntityFrameworkCore;

namespace LaserAPI.Dal
{
    public class LasershowSpotifySpotifyConnectorDal : ILasershowSpotifyConnectorDal
    {
        private readonly DataContext _context;

        public LasershowSpotifySpotifyConnectorDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(LasershowSpotifyConnectorDto connector)
        {
            await _context.LasershowSpotifyConnector.AddAsync(connector);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LasershowSpotifyConnectorDto>> All()
        {
            return await _context.LasershowSpotifyConnector.ToListAsync();
        }

        public async Task<bool> Exists(Guid lasershowUuid)
        {
            return await _context.LasershowSpotifyConnector.AnyAsync(c => c.LasershowUuid == lasershowUuid);
        }

        public async Task Remove(Guid lasershowUuid)
        {
            LasershowSpotifyConnectorDto connector = await _context.LasershowSpotifyConnector.FirstOrDefaultAsync(c => c.LasershowUuid == lasershowUuid);
            if (connector == null)
            {
                return;
            }

            _context.LasershowSpotifyConnector.Remove(connector);
            await _context.SaveChangesAsync();
        }
    }
}
