using LaserAPI.Models.Dto.Zones;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Interfaces.Dal
{
    public interface IZoneDal
    {
        public Task Add(List<ZoneDto> zones);
        public Task<List<ZoneDto>> All();
        public Task Update(ZoneDto zone);
        public Task Remove(Guid uuid);
    }
}
