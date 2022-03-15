using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Lasershow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Dal
{
    public class LasershowDal : ILasershowDal
    {
        private readonly DataContext _context;

        public LasershowDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(LasershowDto lasershow)
        {
            await _context.Lasershow.AddAsync(lasershow);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LasershowDto>> All()
        {
            return await _context.Lasershow
                .Include(e => e.Animations)
                .ToListAsync();
        }

        public async Task Update(LasershowDto lasershow)
        {
            LasershowDto dbLasershow = await _context.Lasershow
                .Include(e => e.Animations)
                .SingleOrDefaultAsync(e => e.Uuid == lasershow.Uuid);

            if (dbLasershow == null)
            {
                throw new KeyNotFoundException();
            }

            dbLasershow.Name = lasershow.Name;
            dbLasershow.Animations = lasershow.Animations;

            _context.Lasershow.Update(dbLasershow);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Guid uuid)
        {
            LasershowDto dbLasershow = await _context.Lasershow
                .Include(e => e.Animations)
                .SingleOrDefaultAsync(e => e.Uuid == uuid);

            if (dbLasershow == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Lasershow.Remove(dbLasershow);
            await _context.SaveChangesAsync();
        }
    }
}
