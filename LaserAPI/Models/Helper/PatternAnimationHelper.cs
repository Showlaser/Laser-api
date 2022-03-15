using LaserAPI.Models.Dto.Animations;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Models.Helper
{
    public static class PatternAnimationHelper
    {
        /// <summary>
        /// Gets the pattern animations the fall between the timeMs value
        /// </summary>
        /// <param name="animation">The animation to search the pattern animations in</param>
        /// <param name="timeMs">The time that the pattern animations need to fall between</param>
        /// <returns>The pattern animations that matches the time, sorted by time</returns>
        public static List<PatternAnimationDto> GetPatternAnimationsBetweenTimeMs(AnimationDto animation, long timeMs)
        {
            return animation.PatternAnimations.FindAll(pa =>
            {
                int startTimeOffset = pa.StartTimeOffset;
                int min = pa.AnimationSettings.MinBy(ast => ast.StartTime).StartTime + startTimeOffset;
                int max = pa.AnimationSettings.MaxBy(ast => ast.StartTime).StartTime + startTimeOffset;

                return timeMs.IsBetweenOrEqualTo(min, max);
            }).OrderBy(pa => pa.StartTimeOffset)
                .ToList();
        }
    }
}
