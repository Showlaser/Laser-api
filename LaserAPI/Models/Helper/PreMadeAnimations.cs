using LaserAPI.Models.Dto.Animations;
using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Helper
{
    public class PreMadeAnimations
    {
        private readonly double _speed;

        public PreMadeAnimations(double speed)
        {
            _speed = speed;
        }

        private static int GenerateRandomValueBetweenBoundaries(int center)
        {
            int randomValue = new Random(Guid.NewGuid().GetHashCode()).Next(-4000, 4000);
            int value = NumberHelper.GetHighestNumber(center, randomValue) + NumberHelper.GetLowestNumber(center, randomValue);
            return FixBoundary(center, value);
        }

        private static int FixBoundary(int center, int value)
        {
            if (value + center > 4000)
            {
                value = 4000;
            }
            if (value + center < -4000)
            {
                value = -4000;
            }

            return value + center;
        }

        private static AnimationPointDto GetRandomPoint(int centerX, int centerY)
        {
            int randomX = GenerateRandomValueBetweenBoundaries(centerX);
            int randomY = GenerateRandomValueBetweenBoundaries(centerY);

            return new AnimationPointDto
            {
                RedLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(7, 255),
                GreenLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(7, 255),
                BlueLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(7, 255),
                X = randomX,
                Y = randomY
            };
        }

        private static AnimationPointDto GetPoint(int x, int y)
        {
            return new AnimationPointDto
            {
                RedLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(7, 255),
                GreenLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(7, 255),
                BlueLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(7, 255),
                X = x,
                Y = y,
            };
        }

        /// <summary>
        /// Returns a list of points that form a rectangle. The rectangle is placed at the center (center x and center y)
        /// </summary>
        /// <param name="width">The width of the rectangle</param>
        /// <param name="height">The height of the rectangle</param>
        /// <returns>A list of points that form a rectangle</returns>
        private static List<AnimationPointDto> GetRectanglePoints(int width, int height, int centerX, int centerY)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentOutOfRangeException(null, nameof(height) + nameof(width));
            }

            return new List<AnimationPointDto>
            {
                GetPoint(FixBoundary(centerX, - width / 2 ), FixBoundary(centerY, - height / 2)),
                GetPoint(FixBoundary(centerX, - width / 2 ), FixBoundary(centerY, height / 2)),
                GetPoint(FixBoundary(centerX, width / 2 ), FixBoundary(centerY, height / 2)),
                GetPoint(FixBoundary(centerX, width / 2 ), FixBoundary(centerY, - height / 2)),
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
                                patternAnimationUuid, centerX, centerY, rotation, scale, Convert.ToInt32(2200 / _speed)),
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
                    patternAnimationUuid, centerX, centerY, rotation, scale, Convert.ToInt32(i * 5 / _speed)));
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

        public AnimationDto GetRectangle(int width, int height, int centerX, int centerY)
        {
            Guid animationUuid = Guid.NewGuid();
            Guid patternAnimationUuid = Guid.NewGuid();
            List<AnimationPointDto> points = new();
            points.AddRange(GetRectanglePoints(width, height, centerX, centerY));

            return new AnimationDto
            {
                Name = "Rectangle",
                Uuid = animationUuid,

                PatternAnimations = new List<PatternAnimationDto>
                {
                    new()
                    {
                        Uuid = patternAnimationUuid,
                        AnimationUuid = animationUuid,
                        Name = "Rectangle",
                        StartTimeOffset = 0,
                        AnimationSettings = new List<PatternAnimationSettingsDto>
                        {
                            GetAnimationSetting(points, patternAnimationUuid, 0, 0, 0, 0, 0),
                            GetAnimationSetting(points, patternAnimationUuid, 0, 0, 0, 0, 16),
                        }
                    }
                }
            };
        }
    }
}
