using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Lasershow;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LasershowLogic
    {
        private readonly ILasershowDal _lasershowDal;

        public LasershowLogic(ILasershowDal lasershowDal)
        {
            _lasershowDal = lasershowDal;
        }

        public async Task AddOrUpdate(LasershowDto lasershow)
        {
            LasershowHelper.LasershowValid(lasershow);
            if (await _lasershowDal.Exists(lasershow))
            {
                await _lasershowDal.Update(lasershow);
                return;
            }

            await _lasershowDal.Add(lasershow);
        }

        public async Task<List<LasershowDto>> All()
        {
            return await _lasershowDal.All();
        }

        public async Task Remove(Guid uuid)
        {
            await _lasershowDal.Remove(uuid);
        }
    }
}
