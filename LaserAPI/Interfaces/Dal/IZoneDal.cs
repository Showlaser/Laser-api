using LaserAPI.Models.Dto.Zones;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Interfaces.Dal
{
    public interface IZoneDal
    {
        public Task Add(SafetyZoneDto zone);
        public Task<List<SafetyZoneDto>> All();
        public Task Update(SafetyZoneDto zone);
        public Task<bool> Exists(Guid uuid);
        public Task Remove(Guid uuid);
    }
}
