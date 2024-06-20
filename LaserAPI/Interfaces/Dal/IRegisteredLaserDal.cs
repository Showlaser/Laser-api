using LaserAPI.Models.Dto.RegisteredLaser;
using System;
using System.Threading.Tasks;

namespace LaserAPI.Interfaces.Dal
{
    public interface IRegisteredLaserDal
    {
        public Task Add(RegisteredLaserDto registeredLaser);
        public Task<RegisteredLaserDto> Find(string laserId);
        public Task Update(RegisteredLaserDto registeredLaser);
        public Task Remove(Guid uuid);
    }
}
