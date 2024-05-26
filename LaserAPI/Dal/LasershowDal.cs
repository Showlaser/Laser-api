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
            await _context.Lasershow.AddAsync(lasershow);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LasershowDto>> All()
        {
            List<LasershowDto> lasershows = await _context.Lasershow
            .Include(e => e.LasershowAnimations)
                .ToListAsync();

            IEnumerable<Guid> animationUuids = lasershows.SelectMany(l => l.LasershowAnimations.Select(la => la.AnimationUuid));
            List<AnimationDto> animations = await _context.Animation.Where(a => animationUuids.Contains(a.Uuid))
                 .Include(a => a.AnimationPatterns)
                 .ThenInclude(pa => pa.AnimationPatternKeyFrames)
                 .Include(pa => pa.AnimationPatterns).ToListAsync();

            IEnumerable<Guid> patternUuids = animations.SelectMany(a => a.AnimationPatterns.Select(ap => ap.PatternUuid));
            List<PatternDto> patterns = await _context.Pattern.Where(p => patternUuids.Contains(p.Uuid)).Include(p => p.Points).ToListAsync();

            int animationsLength = animations.Count;
            for (int i = 0; i < animationsLength; i++)
            {
                AnimationDto animation = animations[i];
                int animationPatternsLength = animation.AnimationPatterns.Count;
                for (int j = 0; j < animationPatternsLength; j++)
                {
                    AnimationPatternDto animationPattern = animation.AnimationPatterns[j];
                    animationPattern.Pattern = patterns.Single(p => p.Uuid == animationPattern.PatternUuid);
                }
            }

            int lasershowsLength = lasershows.Count;
            for (int i = 0; i < lasershowsLength; i++)
            {
                LasershowDto lasershow = lasershows[i];
                int lasershowAnimationsLength = lasershow.LasershowAnimations.Count;
                for (int j = 0; j < lasershowAnimationsLength; j++)
                {
                    LasershowAnimationDto lasershowAnimation = lasershow.LasershowAnimations[j];
                    lasershowAnimation.Animation = animations.Single(a => a.Uuid == lasershowAnimation.AnimationUuid);
                }
            }

            return lasershows;
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

            _context.LasershowAnimation.RemoveRange(dbLasershow.LasershowAnimations);
            await _context.LasershowAnimation.AddRangeAsync(lasershow.LasershowAnimations);
            await _context.SaveChangesAsync();
        }
    }
}
