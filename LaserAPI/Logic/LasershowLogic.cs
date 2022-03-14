using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Lasershow;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LasershowLogic
    {
        private readonly ILasershowDal _lasershowDal;
        private readonly AnimationLogic _animationLogic;

        public LasershowLogic(ILasershowDal lasershowDal, AnimationLogic animationLogic)
        {
            _lasershowDal = lasershowDal;
            _animationLogic = animationLogic;
        }

        public async Task PlayLasershow(LasershowDto lasershow)
        {
            int lasershowDuration = LasershowHelper.GetLasershowDuration(lasershow);
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < lasershowDuration)
            {
                List<LasershowAnimationDto> lasershowAnimationsToPlay =
                    LasershowHelper.GetLasershowAnimationBetweenTimeMs(lasershow, stopwatch.ElapsedMilliseconds,
                        lasershowDuration);
                if (lasershowAnimationsToPlay == null)
                {
                    continue;
                }

                int lasershowAnimationsLength = lasershowAnimationsToPlay.Count;
                for (int i = 0; i < lasershowAnimationsLength; i++)
                {
                    LasershowAnimationDto lasershowAnimation = lasershowAnimationsToPlay[i];
                    await _animationLogic.PlayAnimation(lasershowAnimation.Animation);
                }
            }
        }

        public async Task Add(LasershowDto lasershow)
        {
            LasershowHelper.LasershowValid(lasershow);
            await _lasershowDal.Add(lasershow);
        }

        public async Task Update(LasershowDto lasershow)
        {
            LasershowHelper.LasershowValid(lasershow);
            await _lasershowDal.Update(lasershow);
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
