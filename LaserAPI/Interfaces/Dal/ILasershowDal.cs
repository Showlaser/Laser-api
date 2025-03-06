using LaserAPI.Models.Dto.Lasershows;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Interfaces.Dal
{
    public interface ILasershowDal
    {
        public Task Add(LasershowDto lasershow);
        public Task<List<LasershowDto>> All();
        public Task<bool> Exists(Guid uuid);
        public Task Update(LasershowDto lasershow);
        public Task Remove(Guid uuid);
    }
}
