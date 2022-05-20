using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.LasershowSpotify;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return await _context.LasershowSpotifyConnector.Include(lsc => lsc.SpotifySongs).ToListAsync();
        }

        public async Task<bool> Exists(Guid lasershowUuid)
        {
            return await _context.LasershowSpotifyConnector.AnyAsync(c => c.LasershowUuid == lasershowUuid);
        }

        public async Task<bool> SongsExists(List<string> spotifySongIds)
        {
            return await _context.LasershowSpotifyConnector.AnyAsync(lsc =>
                lsc.SpotifySongs.Any(ss => 
                    spotifySongIds.Contains(ss.SpotifySongId)));
        }

        public async Task Remove(Guid lasershowUuid)
        {
            LasershowSpotifyConnectorDto connector = await _context.LasershowSpotifyConnector.Include(lsc => lsc.SpotifySongs)
                .FirstOrDefaultAsync(c => c.LasershowUuid == lasershowUuid);
            if (connector == null)
            {
                return;
            }

            _context.LasershowSpotifyConnector.Remove(connector);
            await _context.SaveChangesAsync();
        }
    }
}
