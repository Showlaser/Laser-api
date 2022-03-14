using LaserAPI.Models.Dto.Animations;
using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Helper
{
    public class PreMadeAnimations
    {
        private static int GenerateRandomValueBetweenBoundaries(int center)
        {
            int randomValue = new Random(Guid.NewGuid().GetHashCode()).Next(-4000, 4000);
            int value = NumberHelper.GetHighestNumber(center, randomValue) + NumberHelper.GetLowestNumber(center, randomValue);
            if (value > 4000)
            {
                value = 4000;
            }
            if (value < -4000)
            {
                value = -4000;
            }

            return value;
        }

        private static AnimationPointDto GetRandomPoint(int centerX, int centerY)
        {
            int randomX = GenerateRandomValueBetweenBoundaries(centerX);
            int randomY = GenerateRandomValueBetweenBoundaries(centerY);

            return new AnimationPointDto
            {
                RedLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(0, 255),
                GreenLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(0, 255),
                BlueLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(0, 255),
                X = randomX,
                Y = randomY
            };
        }

        private static AnimationPointDto GetPoint(int x, int y)
        {
            return new AnimationPointDto
            {
                RedLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(0, 255),
                GreenLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(0, 255),
                BlueLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(0, 255),
                X = x,
                Y = y,
            };
        }

        private static PatternAnimationSettingsDto GetAnimationSetting(List<AnimationPointDto> points,
            Guid patternAnimationUuid, int centerX, int centerY, int rotation, double scale, int startTime)
        {
            return new PatternAnimationSettingsDto
            {
                Uuid = Guid.NewGuid(),
                PatternAnimationUuid = patternAnimationUuid,
                CenterX = centerX,
                CenterY = centerY,
                Rotation = rotation,
                Scale = scale,
                StartTime = startTime,
                Points = points
            };
        }

        public AnimationDto LineAnimation(int centerX, int centerY, int rotation, double scale)
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
                                patternAnimationUuid, centerX, centerY, rotation, scale, 0),
                            GetAnimationSetting(new List<AnimationPointDto>
                                {
                                    GetPoint(-4000, 0),
                                    GetPoint(4000, 0),
                                },
                                patternAnimationUuid, centerX, centerY, rotation, scale, 100),
                        }
                    }
                }
            };
        }

        public AnimationDto RandomPoints(int centerX, int centerY, int rotation, double scale)
        {
            Guid animationUuid = Guid.NewGuid();
            Guid patternAnimationUuid = Guid.NewGuid();

            List<PatternAnimationSettingsDto> settings = new();
            for (int i = 0; i < 200; i++)
            {
                settings.Add(GetAnimationSetting(new List<AnimationPointDto>
                    {
                        GetRandomPoint(centerX, centerY),
                    },
                    patternAnimationUuid, centerX, centerY, rotation, scale, i + 2));
            }

            return new AnimationDto
            {
                Name = "PreMade random dots",
                Uuid = animationUuid,
                PatternAnimations = new List<PatternAnimationDto>
                {
                    new()
                    {
                        Uuid = patternAnimationUuid,
                        AnimationUuid = animationUuid,
                        Name = "Random dots",
                        StartTimeOffset = 0,
                        TimeLineId = 0,
                        AnimationSettings = settings
                    }
                }
            };
        }
    }
}
