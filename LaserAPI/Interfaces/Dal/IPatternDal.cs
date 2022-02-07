using LaserAPI.Models.Dto.Patterns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Interfaces.Dal
{
    public interface IPatternDal
    {
        public Task Add(PatternDto pattern);
        public Task<List<PatternDto>> All();
        public Task<bool> Exists(Guid uuid);
        public Task Update(PatternDto pattern);
        public Task Remove(Guid uuid);
    }
}