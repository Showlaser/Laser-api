using LaserAPI.Logic;
using LaserAPI.Models.Dto.Animations;
using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Helper.LaserAnimations
{
    public class CircleAnimation : PreMadeAnimation, IPreMadeLaserAnimation
    {
        public string AnimationName => "CircleAnimation";

        public AnimationDto GetAnimation(PreMadeAnimationOptions options)
        {
            AnimationPointDto point = GetPoint(0, Convert.ToInt32(4000 * options.Scale));
            PatternAnimationSettingsDto patternAnimationSettings = new()
            {
                CenterX = options.CenterX,
                CenterY = options.CenterY
            };

            List<AnimationPointDto> rotatedPoints = new();
            for (int i = 0; i < 36; i++)
            {
                patternAnimationSettings.Rotation = i * 10;
                AnimationPointDto rotatedPoint = AnimationLogic.RotatePoint(point, patternAnimationSettings);
                rotatedPoint.X = FixBoundary(patternAnimationSettings.CenterX, rotatedPoint.X);
                rotatedPoint.Y = FixBoundary(patternAnimationSettings.CenterY, rotatedPoint.Y);
                rotatedPoint.Order = i;
                rotatedPoint.Uuid = Guid.NewGuid();
                rotatedPoints.Add(rotatedPoint);
            }

            Guid animationUuid = Guid.NewGuid();
            Guid patternAnimationUuid = Guid.NewGuid();

            return new AnimationDto
            {
                Name = "PreMade circle",
                Uuid = animationUuid,
                AnimationPatterns = new List<AnimationPatternDto>
                {
                    new()
                    {
                        Uuid = patternAnimationUuid,
                        AnimationUuid = animationUuid,
                        Name = "Circle",
                        StartTimeOffset = 0,
                        TimeLineId = 0,
                        AnimationSettings = new List<PatternAnimationSettingsDto>
                        {
                            GetAnimationSetting(rotatedPoints,
                                patternAnimationUuid, options.CenterX, options.CenterY, options.Rotation, options.Scale, 0),
                            GetAnimationSetting(rotatedPoints,
                                patternAnimationUuid, options.CenterX, options.CenterY, options.Rotation, options.Scale, Convert.ToInt32(2200 / options.Speed)),
                        }
                    }
                }
            };
        }
    }
}
