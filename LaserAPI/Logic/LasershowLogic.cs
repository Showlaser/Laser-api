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
    }
}
