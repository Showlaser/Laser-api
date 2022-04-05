using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Lasershow;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            LasershowValid(lasershow);
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
            int lasershowDuration = GetLasershowDuration(lasershow);
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < lasershowDuration)
            {
                List<LasershowAnimationDto> lasershowAnimationsToPlay =
                    GetLasershowAnimationBetweenTimeMs(lasershow, stopwatch.ElapsedMilliseconds,
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

        public static bool LasershowValid(LasershowDto lasershow)
        {
            return !string.IsNullOrEmpty(lasershow.Name) && lasershow.Animations.TrueForAll(a =>
                a.StartTimeOffset >= 0 &&
                !string.IsNullOrEmpty(a.Name) &&
                a.TimeLineId.IsBetweenOrEqualTo(0, 3) &&
                AnimationLogic.AnimationValid(a.Animation));
        }

        public static int GetLasershowDuration(LasershowDto lasershow)
        {
            PatternAnimationDto maxStartTime = lasershow.Animations.MaxBy(a => a.StartTimeOffset).Animation
                .PatternAnimations.MaxBy(pa => pa.StartTimeOffset);

            return maxStartTime.AnimationSettings.MaxBy(ast => ast.StartTime).StartTime +
                   maxStartTime.StartTimeOffset;
        }

        public static List<LasershowAnimationDto> GetLasershowAnimationBetweenTimeMs(LasershowDto lasershow, long timeMs, int lasershowLength)
        {
            return lasershow.Animations.FindAll(a =>
                    timeMs.IsBetweenOrEqualTo(timeMs, lasershowLength))
                .OrderBy(ls => ls.StartTimeOffset)
                .ToList();
        }
    }
}
