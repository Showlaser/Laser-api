using LaserAPI.Models.Dto.RegisteredLaser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Interfaces.Dal
{
    public interface IRegisteredLaserDal
    {
        public Task Add(RegisteredLaserDto registeredLaser);
        public Task<List<RegisteredLaserDto>> All();
        public Task<RegisteredLaserDto> Find(Guid uuid);
        public Task Update(RegisteredLaserDto registeredLaser);
        public Task Remove(Guid uuid);
    }
}
