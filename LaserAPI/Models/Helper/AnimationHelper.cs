using LaserAPI.Models.Dto.Animations;
using System.Linq;

namespace LaserAPI.Models.Helper
{
    public static class AnimationHelper
    {
        public static int GetAnimationDuration(AnimationDto animation)
        {
            PatternAnimationDto maxStartTime = animation.PatternAnimations.MaxBy(pa => pa.StartTimeOffset);
            return maxStartTime.AnimationSettings.MaxBy(ast => ast.StartTime).StartTime +
                                maxStartTime.StartTimeOffset;
        }
    }
}
