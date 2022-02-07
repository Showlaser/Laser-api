using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class ZoneLogic
    {
        private readonly IZoneDal _zoneDal;

        public ZoneLogic(IZoneDal zoneDal)
        {
            _zoneDal = zoneDal;
        }

        public async Task<List<ZoneDto>> All()
        {
            return await _zoneDal.All();
        }

        public async Task AddOrUpdate(ZoneDto zone)
        {
            if (await _zoneDal.Exists(zone.Uuid))
            {
                await _zoneDal.Update(zone);
                return;
            }

            await _zoneDal.Add(zone);
        }

        public async Task Remove(Guid uuid)
        {
            await _zoneDal.Remove(uuid);
        }
    }
}
