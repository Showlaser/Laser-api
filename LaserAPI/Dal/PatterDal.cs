using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Patterns;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Dal
{
    public class PatterDal : IPatternDal
    {
        private readonly DataContext _context;

        public PatterDal(DataContext context)
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

        public async Task Update(PatternDto pattern)
        {
            _context.Pattern.Update(pattern);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Guid uuid)
        {
            PatternDto pattern = await _context.Pattern.FindAsync(uuid);
            if (pattern == null)
            {
                throw new KeyNotFoundException(nameof(uuid));
            }

            _context.Pattern.Remove(pattern);
            await _context.SaveChangesAsync();
        }
    }
}