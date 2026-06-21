using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Lasershows;
using LaserAPI.Models.Dto.Patterns;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Dal
{
    public class LasershowDal(DataContext _context) : ILasershowDal
    {
        public async Task Add(LasershowDto lasershow)
        {
            DetachSharedAnimations(lasershow);
            await _context.Lasershow.AddAsync(lasershow);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Animations are shared library items referenced by AnimationUuid. Null the navigation
        /// so EF only persists the foreign key and does not try to re-insert the animation.
        /// </summary>
        private static void DetachSharedAnimations(LasershowDto lasershow)
        {
            foreach (LasershowAnimationDto lasershowAnimation in lasershow.LasershowAnimations)
            {
                lasershowAnimation.Animation = null;
            }
        }

        public async Task<List<LasershowDto>> All()
        {
            return await _context.Lasershow
                .Include(l => l.LasershowAnimations)
                    .ThenInclude(la => la.Animation)
                        .ThenInclude(a => a.AnimationPatterns)
                            .ThenInclude(ap => ap.AnimationPatternKeyFrames)
                .Include(l => l.LasershowAnimations)
                    .ThenInclude(la => la.Animation)
                        .ThenInclude(a => a.AnimationPatterns)
                            .ThenInclude(ap => ap.Pattern)
                                .ThenInclude(p => p.Points)
                .ToListAsync();
        }

        public Task<bool> Exists(Guid uuid)
        {
            return _context.Lasershow.AnyAsync(e => e.Uuid == uuid);
        }

        public async Task Remove(Guid uuid)
        {
            LasershowDto lasershowToRemove = await _context.Lasershow
                .FirstOrDefaultAsync(e => e.Uuid == uuid) ?? throw new KeyNotFoundException();

            _context.Lasershow.Remove(lasershowToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task Update(LasershowDto lasershow)
        {
            LasershowDto dbLasershow = await _context.Lasershow
                .Include(e => e.LasershowAnimations)
                .SingleOrDefaultAsync(e => e.Uuid == lasershow.Uuid) ?? throw new KeyNotFoundException();

            dbLasershow.Name = lasershow.Name;
            dbLasershow.Image = lasershow.Image;
            _context.Lasershow.Update(dbLasershow);

            DetachSharedAnimations(lasershow);
            _context.LasershowAnimation.RemoveRange(dbLasershow.LasershowAnimations);
            await _context.LasershowAnimation.AddRangeAsync(lasershow.LasershowAnimations);
            await _context.SaveChangesAsync();
        }
    }
}
