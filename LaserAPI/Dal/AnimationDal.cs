using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Patterns;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Dal
{
    public class AnimationDal : IAnimationDal
    {
        private readonly DataContext _context;

        public AnimationDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(AnimationDto animation)
        {
            DetachSharedPatterns(animation);
            await _context.Animation.AddAsync(animation);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Patterns are shared library items referenced by PatternUuid. Null the navigation
        /// so EF only persists the foreign key and does not try to re-insert the pattern.
        /// </summary>
        private static void DetachSharedPatterns(AnimationDto animation)
        {
            foreach (AnimationPatternDto animationPattern in animation.AnimationPatterns)
            {
                animationPattern.Pattern = null;
            }
        }

        private async Task<AnimationDto> Find(Guid uuid)
        {
            return await _context.Animation
                .Include(a => a.AnimationPatterns)
                .ThenInclude(pa => pa.AnimationPatternKeyFrames)
                .SingleOrDefaultAsync(a => a.Uuid == uuid);
        }

        public async Task<List<AnimationDto>> All()
        {
            return await _context.Animation
                .Include(a => a.AnimationPatterns)
                    .ThenInclude(ap => ap.AnimationPatternKeyFrames)
                .Include(a => a.AnimationPatterns)
                    .ThenInclude(ap => ap.Pattern)
                        .ThenInclude(p => p.Points)
                .ToListAsync();
        }

        public async Task<bool> Exists(Guid uuid)
        {
            return await _context.Animation.AnyAsync(a => a.Uuid == uuid);
        }

        public async Task Update(AnimationDto animation)
        {
            AnimationDto dbAnimation = await _context.Animation
                .Include(a => a.AnimationPatterns)
                .ThenInclude(pa => pa.AnimationPatternKeyFrames)
                .SingleOrDefaultAsync(a => a.Uuid == animation.Uuid) ?? throw new NoNullAllowedException();

            dbAnimation.Name = animation.Name;
            dbAnimation.Image = animation.Image;
            _context.Animation.Update(dbAnimation);

            DetachSharedPatterns(animation);
            _context.PatternAnimation.RemoveRange(dbAnimation.AnimationPatterns);
            await _context.PatternAnimation.AddRangeAsync(animation.AnimationPatterns);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Guid uuid)
        {
            AnimationDto animationToRemove = await Find(uuid);
            if (animationToRemove == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Animation.Remove(animationToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
