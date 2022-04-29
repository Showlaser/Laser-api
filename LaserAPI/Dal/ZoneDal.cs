using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Dal
{
    public class ZoneDal : IZoneDal
    {
        private readonly DataContext _context;

        public ZoneDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(ZoneDto zone)
        {
            await _context.Zone.AddAsync(zone);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ZoneDto>> All()
        {
            return await _context.Zone
                .Include(e => e.Points)
                .ToListAsync();
        }

        public async Task Update(ZoneDto zone)
        {
            ZoneDto zoneToUpdate = await _context.Zone
                .Include(z => z.Points)
                .SingleOrDefaultAsync(z => z.Uuid == zone.Uuid);

            zoneToUpdate.Name = zone.Name;
            zoneToUpdate.MaxLaserPowerInZonePwm = zone.MaxLaserPowerInZonePwm;
            _context.ZonePosition.RemoveRange(zoneToUpdate.Points);
            await _context.ZonePosition.AddRangeAsync(zone.Points);
            _context.Zone.Update(zoneToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(Guid uuid)
        {
            return await _context.Zone.AnyAsync(z => z.Uuid == uuid);
        }

        public async Task Remove(Guid uuid)
        {
            ZoneDto zoneToUpdate = await _context.Zone
                .Where(z => z.Uuid == uuid)
                .Include(z => z.Points)
                .FirstOrDefaultAsync();

            if (zoneToUpdate == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Zone.Remove(zoneToUpdate);
            await _context.SaveChangesAsync();
        }
    }
}
