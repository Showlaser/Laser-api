using LaserAPI.Logic;
using LaserAPI.Models.Dto.Animations;
using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Helper.LaserAnimations
{
    public class CircleAnimation : PreMadeAnimation, IPreMadeLaserAnimation
    {
        public double Speed { get; set; }
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
            int previous = 0;
            for (int i = 0; i < 360; i++)
            {
                if (previous > 10)
                {
                    patternAnimationSettings.Rotation = i;
                    AnimationPointDto rotatedPoint = AnimationLogic.RotatePoint(point, patternAnimationSettings);
                    rotatedPoint.Order = i;
                    rotatedPoints.Add(rotatedPoint);
                    previous = 0;
                }

                previous++;
            }

            Guid animationUuid = Guid.NewGuid();
            Guid patternAnimationUuid = Guid.NewGuid();

            return new AnimationDto
            {
                Name = "PreMade circle",
                Uuid = animationUuid,
                PatternAnimations = new List<PatternAnimationDto>
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
                                patternAnimationUuid, options.CenterX, options.CenterY, options.Rotation, options.Scale, Convert.ToInt32(1800 / Speed)),
                        }
                    }
                }
            };
        }
    }
}
