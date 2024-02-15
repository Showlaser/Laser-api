using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Lasershows;
using LaserAPI.Models.Dto.Patterns;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Logic
{
    public static class LasershowRenderEngine
    {
        public static IEnumerable<LasershowAnimationDto> GetLasershowAnimationsInTimelinePosition(LasershowDto lasershow, int timelinePosition) =>
            lasershow.LasershowAnimations.Where(la => la.StartTimeMs >= timelinePosition);

        public static IEnumerable<AnimationPatternDto> GetAnimationPatternsEqualOrAfterTimelinePosition(IEnumerable<LasershowAnimationDto> lasershowAnimationPatterns,
            int timelinePosition)
        {
            return lasershowAnimationPatterns.SelectMany(lsap => lsap.Animation.AnimationPatterns
                                             .Where(ap => ap.StartTimeMs >= timelinePosition));
        }

        public static int GetAnimationPatternDuration(AnimationPatternDto animationPattern)
        {
            return animationPattern.AnimationPatternKeyFrames.MaxBy(apkf => apkf.TimeMs).TimeMs + animationPattern.StartTimeMs;
        }

        public static int GetLasershowAnimationDuration(LasershowAnimationDto lasershowAnimation)
        {
            return lasershowAnimation.Animation.AnimationPatterns
                .Max(ap => ap.StartTimeMs + GetAnimationPatternDuration(ap)) + lasershowAnimation.StartTimeMs;
        }

        public static int GetLasershowDuration(LasershowDto lasershow)
        {
            IEnumerable<int> animationDurations = lasershow.LasershowAnimations.Select(ls => ls.StartTimeMs + GetLasershowAnimationDuration(ls));
            return animationDurations.Max();
        }

        public static IEnumerable<PointDto> GetPointsToDraw(LasershowDto lasershow, int timelinePosition)
        {
            IEnumerable<LasershowAnimationDto> lasershowAnimations = GetLasershowAnimationsInTimelinePosition(lasershow, timelinePosition);
            IEnumerable<AnimationPatternDto> animationPatterns = GetAnimationPatternsEqualOrAfterTimelinePosition(lasershowAnimations, timelinePosition);

            foreach (AnimationPatternDto pattern in animationPatterns)
            {

            }

            return [];
        }
    }
}
