using LaserAPI.Models.Dto.Animations;
using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Helper.LaserAnimations
{
    public class LineAnimation : PreMadeAnimation, IPreMadeLaserAnimation
    {
        public string AnimationName => "LineAnimation";

        public AnimationDto GetAnimation(PreMadeAnimationOptions options)
        {
            Guid animationUuid = Guid.NewGuid();
            Guid patternAnimationUuid = Guid.NewGuid();

            return new AnimationDto
            {
                Name = "PreMade line",
                Uuid = animationUuid,
                PatternAnimations = new List<PatternAnimationDto>
                {
                    new()
                    {
                        Uuid = patternAnimationUuid,
                        AnimationUuid = animationUuid,
                        Name = "Line",
                        StartTimeOffset = 0,
                        TimeLineId = 0,
                        AnimationSettings = new List<PatternAnimationSettingsDto>
                        {
                            GetAnimationSetting(new List<AnimationPointDto>
                                {
                                    GetPoint(-4000, 0),
                                    GetPoint(4000, 0),
                                },
                                patternAnimationUuid, options.CenterX, options.CenterY, options.Rotation, options.Scale, 0),
                            GetAnimationSetting(new List<AnimationPointDto>
                                {
                                    GetPoint(-4000, 0),
                                    GetPoint(4000, 0),
                                },
                                patternAnimationUuid, options.CenterX, options.CenterY, options.Rotation, options.Scale, Convert.ToInt32(2200 / options.Speed)),
                        }
                    }
                }
            };
        }
    }
}
