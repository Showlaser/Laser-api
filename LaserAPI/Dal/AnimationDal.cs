using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Animations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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
                .Include(a => a.PatternAnimations)
                .ThenInclude(pa => pa.AnimationSettings)
                .ThenInclude(ast => ast.Points)
                .SingleOrDefaultAsync(a => a.Uuid == uuid);
        }

        public async Task<List<AnimationDto>> All()
        {
            return await _context.Animation
                .Include(a => a.PatternAnimations)
                .ThenInclude(pa => pa.AnimationSettings)
                .ThenInclude(ast => ast.Points)
                .ToListAsync();
        }

        public async Task<bool> Exists(Guid uuid)
        {
            return await _context.Animation.AnyAsync(a => a.Uuid == uuid);
        }

        public async Task Update(AnimationDto animation)
        {
            AnimationDto dbAnimation = await _context.Animation.Include(a => a.PatternAnimations)
                .ThenInclude(pa => pa.AnimationSettings)
                .ThenInclude(ast => ast.Points)
                .AsNoTrackingWithIdentityResolution()
                .SingleOrDefaultAsync(a => a.Uuid == animation.Uuid);

            if (dbAnimation == null)
            {
                throw new NoNullAllowedException();
            }

            dbAnimation.Name = animation.Name;
            _context.Animation.Update(dbAnimation);

            _context.PatternAnimation.RemoveRange(dbAnimation.PatternAnimations);
            await _context.PatternAnimation.AddRangeAsync(animation.PatternAnimations);
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
