using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Lasershow;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Models.Helper
{
    public static class LasershowHelper
    {
        public static bool LasershowValid(LasershowDto lasershow)
        {
            return !string.IsNullOrEmpty(lasershow.Name) && lasershow.Animations.TrueForAll(a =>
                a.StartTime >= 0 &&
                !string.IsNullOrEmpty(a.Name) &&
                a.TimelineId.IsBetweenOrEqualTo(0, 3) &&
                AnimationHelper.AnimationValid(a.Animation));
        }

        public static int GetLasershowDuration(LasershowDto lasershow)
        {
            PatternAnimationDto maxStartTime = lasershow.Animations.MaxBy(a => a.StartTime).Animation
                .PatternAnimations.MaxBy(pa => pa.StartTimeOffset);

            return maxStartTime.AnimationSettings.MaxBy(ast => ast.StartTime).StartTime +
                   maxStartTime.StartTimeOffset;
        }

        public static List<LasershowAnimationDto> GetLasershowAnimationBetweenTimeMs(LasershowDto lasershow, long timeMs, int lasershowLength)
        {
            return lasershow.Animations.FindAll(a =>
                timeMs.IsBetweenOrEqualTo(timeMs, lasershowLength))
                .OrderBy(ls => ls.StartTime)
                .ToList();
        }
    }
}
