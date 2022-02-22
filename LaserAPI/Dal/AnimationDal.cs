using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Animations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            _context.Animation.Update(animation);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Guid uuid)
        {
            AnimationDto animationToRemove = (await _context.Animation.Where(a => a.Uuid == uuid)
                .Include(a => a.PatternAnimations)
                .ThenInclude(pa => pa.AnimationSettings)
                .ThenInclude(ast => ast.Points)
                .ToListAsync()).FirstOrDefault();

            if (animationToRemove == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Animation.Remove(animationToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
