using LaserAPI.Models.Dto.Animations;
using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Helper
{
    public class PreMadeAnimation
    {
        internal static int GenerateRandomValueBetweenBoundaries(int center)
        {
            int randomValue = new Random(Guid.NewGuid().GetHashCode()).Next(-4000, 4000);
            int value = NumberHelper.GetHighestNumber(center, randomValue) + NumberHelper.GetLowestNumber(center, randomValue);
            return FixBoundary(center, value);
        }

        /// <summary>
        /// Checks if the value plus the center do not exceed 4000
        /// </summary>
        /// <param name="center"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static int FixBoundary(int center, int value)
        {
            if (value + center > 4000)
            {
                value = 4000;
            }
            if (value + center < -4000)
            {
                value = -4000;
            }

            return value;
        }

        internal static AnimationPointDto GetRandomPoint(int centerX, int centerY)
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

        internal static AnimationPointDto GetPoint(int x, int y)
        {
            return new AnimationPointDto
            {
                Uuid = Guid.NewGuid(),
                RedLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(7, 255),
                GreenLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(7, 255),
                BlueLaserPowerPwm = new Random(Guid.NewGuid().GetHashCode()).Next(7, 255),
                X = FixBoundary(0, x),
                Y = FixBoundary(0, y),
            };
        }

        /// <summary>
        /// Returns a list of points that form a rectangle. The rectangle is placed at the center (center x and center y)
        /// </summary>
        /// <param name="width">The width of the rectangle</param>
        /// <param name="height">The height of the rectangle</param>
        /// <returns>A list of points that form a rectangle</returns>
        internal static List<AnimationPointDto> GetRectanglePoints(int width, int height, int centerX, int centerY)
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

        internal static PatternAnimationSettingsDto GetAnimationSetting(List<AnimationPointDto> points,
            Guid patternAnimationUuid, int centerX, int centerY, int rotation, double scale, int startTime)
        {
            Guid uuid = Guid.NewGuid();
            int pointsLength = points.Count;
            for (int i = 0; i < pointsLength; i++)
            {
                AnimationPointDto point = points[i];
                point.PatternAnimationSettingsUuid = uuid;
            }

            return new PatternAnimationSettingsDto
            {
                Uuid = uuid,
                PatternAnimationUuid = patternAnimationUuid,
                CenterX = centerX,
                CenterY = centerY,
                Rotation = rotation,
                Scale = scale,
                StartTime = startTime,
                Points = points
            };
        }
    }
}
