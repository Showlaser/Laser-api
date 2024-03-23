using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Lasershows;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LasershowLogic(ILasershowDal _lasershowDal)
    {
        private static bool LasershowIsValid(LasershowDto lasershowDto)
        {
            bool lasershowIsValid = lasershowDto.Uuid != Guid.Empty &&
                !string.IsNullOrEmpty(lasershowDto.Image) &&
                !string.IsNullOrEmpty(lasershowDto.Name);

            bool lasershowAnimationsAreValid = lasershowDto.LasershowAnimations.TrueForAll(la =>
            la.Uuid != Guid.Empty &&
            la.AnimationUuid != Guid.Empty &&
            la.LasershowUuid == lasershowDto.Uuid &&
            la.TimelineId.IsBetweenOrEqualTo(0, 3) &&
            la.StartTimeMs >= 0 &&
            AnimationLogic.AnimationIsValid(la.Animation));

            return lasershowIsValid && lasershowAnimationsAreValid;
        }

        public async Task AddOrUpdate(LasershowDto lasershow)
        {
            if (!LasershowIsValid(lasershow))
            {
                throw new InvalidDataException();
            }

            if (await _lasershowDal.Exists(lasershow.Uuid))
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
