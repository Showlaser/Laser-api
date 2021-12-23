using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Patterns;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class PatternLogic
    {
        private readonly IPatternDal _patternDal;

        public PatternLogic(IPatternDal patternDal)
        {
            _patternDal = patternDal;
        }

        private bool ValidatePoints(List<PointDto> points)
        {
            return points.Any() && points.TrueForAll(p => p.PatternUuid != Guid.Empty);
        }

        private void ValidatePattern(PatternDto pattern)
        {
            bool patternValid = pattern != null && pattern.Scale.IsBetweenOrEqualTo(0.1, 1) && ValidatePoints(pattern.Points);
            if (!patternValid)
            {
                throw new InvalidDataException(nameof(pattern));
            };
        }

        public async Task Add(PatternDto pattern)
        {
            ValidatePattern(pattern);
            await _patternDal.Add(pattern);
        }

        public async Task<List<PatternDto>> All()
        {
            return await _patternDal.All();
        }

        public async Task Update(PatternDto pattern)
        {
            ValidatePattern(pattern);
            await _patternDal.Update(pattern);
        }

        public async Task Remove(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                throw new NoNullAllowedException(nameof(uuid));
            }
            await _patternDal.Remove(uuid);
        }
    }
}
