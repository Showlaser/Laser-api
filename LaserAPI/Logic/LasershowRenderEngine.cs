using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Lasershows;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Logic
{
    public static class LasershowRenderEngine
    {
        public static IEnumerable<LasershowAnimationDto> GetLasershowAnimationEqualOrAfterTimelinePosition(LasershowDto mockedLasershow, int timelinePosition) =>
            mockedLasershow.LasershowAnimations.Where(la => la.StartTimeMs >= timelinePosition);

        public static IEnumerable<AnimationPatternDto> GetLasershowAnimationPatternsEqualOrAfterTimelinePosition(IEnumerable<AnimationPatternDto> animationPatterns, int timelinePosition) =>
            animationPatterns.Where(ap => ap.StartTimeMs >= timelinePosition);
    }
}
