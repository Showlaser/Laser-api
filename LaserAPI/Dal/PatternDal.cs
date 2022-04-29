using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Patterns;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Dal
{
    public class PatternDal : IPatternDal
    {
        private readonly DataContext _context;

        public PatternDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(PatternDto pattern)
        {
            await _context.Pattern.AddAsync(pattern);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PatternDto>> All()
        {
            return await _context.Pattern
                .Include(p => p.Points)
                .ToListAsync();
        }

        public async Task<bool> Exists(Guid uuid)
        {
            return await _context.Pattern.AnyAsync(p => p.Uuid == uuid);
        }

        public async Task Update(PatternDto pattern)
        {
            PatternDto patternToUpdate = await _context.Pattern.Include(p => p.Points)
                .SingleOrDefaultAsync(p => p.Uuid == pattern.Uuid);

            patternToUpdate.Name = pattern.Name;
            patternToUpdate.Scale = pattern.Scale;
            _context.Point.RemoveRange(patternToUpdate.Points);
            await _context.Point.AddRangeAsync(pattern.Points);

            _context.Pattern.Update(patternToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Guid uuid)
        {
            PatternDto pattern = (await _context.Pattern
                .Where(p => p.Uuid == uuid)
                .Include(p => p.Points)
                .ToListAsync())
                .FirstOrDefault();

            if (pattern == null)
            {
                throw new KeyNotFoundException(nameof(uuid));
            }

            _context.Pattern.Remove(pattern);
            await _context.SaveChangesAsync();
        }
    }
}