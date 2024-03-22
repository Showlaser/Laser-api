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
            await _context.Animation.AddAsync(animation);
            await _context.SaveChangesAsync();
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
            List<AnimationDto> animations = await _context.Animation
                .Include(a => a.AnimationPatterns)
                .ThenInclude(pa => pa.AnimationPatternKeyFrames)
                .Include(pa => pa.AnimationPatterns).ToListAsync();

            IEnumerable<Guid> patternUuids = animations.SelectMany(a => a.AnimationPatterns.Select(ap => ap.PatternUuid));
            List<PatternDto> patterns = await _context.Pattern.Where(p => patternUuids.Contains(p.Uuid)).ToListAsync();

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

            return animations;
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
                .AsNoTrackingWithIdentityResolution()
                .SingleOrDefaultAsync(a => a.Uuid == animation.Uuid) ?? throw new NoNullAllowedException();

            dbAnimation.Name = animation.Name;
            dbAnimation.Image = animation.Image;
            _context.Animation.Update(dbAnimation);

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
