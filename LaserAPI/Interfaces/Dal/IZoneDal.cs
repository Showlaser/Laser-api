using LaserAPI.Models.Dto.Zones;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Interfaces.Dal
{
    public interface IZoneDal
    {
        public Task Add(ZoneDto zone);
        public Task<List<ZoneDto>> All();
        public Task Update(ZoneDto zone);
        public Task<bool> Exists(Guid uuid);
        public Task Remove(Guid uuid);
    }
}
