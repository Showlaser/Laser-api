using LaserAPI.Models.Dto.Animations;
using System;
using System.Collections.Generic;
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

        private static bool AnimationDoesNotContainsSettingsWithSameStartTime(AnimationDto animation)
        {
            List<int> startTimeCollection = animation.PatternAnimations
                .SelectMany(pa => pa.AnimationSettings
                    .Select(ase => ase.StartTime))
                .ToList();

            return startTimeCollection.Distinct().Any();
        }

        public static bool AnimationValid(AnimationDto animation)
        {
            return animation != null &&
                                  animation.PatternAnimations
                                      .TrueForAll(pa => pa.AnimationSettings
                                          .TrueForAll(ase => SettingsValid(pa.AnimationSettings))) &&
                                  animation.PatternAnimations
                                      .TrueForAll(pa => pa.AnimationSettings
                                          .TrueForAll(ase => PointsValid(ase.Points))) &&
                                  PatternAnimationValid(animation.PatternAnimations) &&
                                  AnimationDoesNotContainsSettingsWithSameStartTime(animation);
        }

        private static bool PointsValid(List<AnimationPointDto> points)
        {
            return points.Any() && points.TrueForAll(p => p.PatternAnimationSettingsUuid != Guid.Empty &&
                                                          p.Y.IsBetweenOrEqualTo(-4000, 4000) &&
                                                          p.X.IsBetweenOrEqualTo(-4000, 4000) &&
                                                          p.RedLaserPowerPwm.IsBetweenOrEqualTo(0, 511) &&
                                                          p.GreenLaserPowerPwm.IsBetweenOrEqualTo(0, 511) &&
                                                          p.BlueLaserPowerPwm.IsBetweenOrEqualTo(0, 511));
        }

        private static bool SettingsValid(List<PatternAnimationSettingsDto> settings)
        {
            return settings.TrueForAll(setting => setting.CenterX.IsBetweenOrEqualTo(-4000, 4000) &&
                                                  setting.CenterY.IsBetweenOrEqualTo(-4000, 4000) &&
                                                  setting.Scale.IsBetweenOrEqualTo(0.1, 1));
        }

        private static bool PatternAnimationValid(List<PatternAnimationDto> patternAnimations)
        {
            return patternAnimations.TrueForAll(patternAnimation =>
                patternAnimation.AnimationUuid != Guid.Empty &&
                patternAnimation.TimeLineId.IsBetweenOrEqualTo(0, 3) &&
                patternAnimation.Uuid != Guid.Empty);
        }
    }
}
