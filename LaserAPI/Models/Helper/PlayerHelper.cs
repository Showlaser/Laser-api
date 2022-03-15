using LaserAPI.Logic;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Lasershow;
using LaserAPI.Models.Helper.Laser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Models.Helper
{
    public static class PlayerHelper
    {
        public static async Task PlayLasershow(LasershowDto lasershow)
        {
            int lasershowDuration = LasershowHelper.GetLasershowDuration(lasershow);
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < lasershowDuration)
            {
                List<LasershowAnimationDto> lasershowAnimationsToPlay =
                    LasershowHelper.GetLasershowAnimationBetweenTimeMs(lasershow, stopwatch.ElapsedMilliseconds,
                        lasershowDuration);
                if (lasershowAnimationsToPlay == null)
                {
                    continue;
                }

                int lasershowAnimationsLength = lasershowAnimationsToPlay.Count;
                for (int i = 0; i < lasershowAnimationsLength; i++)
                {
                    LasershowAnimationDto lasershowAnimation = lasershowAnimationsToPlay[i];
                    await PlayAnimation(lasershowAnimation.Animation);
                }
            }
        }

        public static async Task PlayAnimation(AnimationDto animation)
        {
            int animationDuration = AnimationHelper.GetAnimationDuration(animation);
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < animationDuration)
            {
                List<PatternAnimationDto> patternAnimationsToPlay = PatternAnimationHelper
                    .GetPatternAnimationsBetweenTimeMs(animation, stopwatch.ElapsedMilliseconds);
                if (patternAnimationsToPlay == null)
                {
                    continue;
                }

                List<PatternAnimationSettingsDto> settingsToPlay = new();
                int patternAnimationsLength = patternAnimationsToPlay.Count;

                long stopwatchTime = stopwatch.ElapsedMilliseconds;
                for (int i = 0; i < patternAnimationsLength; i++)
                {
                    PatternAnimationDto patternAnimation = patternAnimationsToPlay[i];
                    PatternAnimationSettingsDto closestPatternAnimationSettings = PatternSettingsHelper
                        .GetSettingClosestToTimeMs(patternAnimation.AnimationSettings, patternAnimation.StartTimeOffset,
                            stopwatchTime);

                    if (closestPatternAnimationSettings != null)
                    {
                        settingsToPlay.Add(closestPatternAnimationSettings);
                    }
                }

                if (!settingsToPlay.Any())
                {
                    continue;
                }

                int settingsToPlayLength = settingsToPlay.Count;

                for (int i = 0; i < settingsToPlayLength; i++)
                {
                    PatternAnimationSettingsDto settingToPlay = settingsToPlay[i];

                    int pointsCount = settingToPlay.Points.Count;
                    for (int k = 0; k < pointsCount; k++)
                    {
                        AnimationPointDto point = settingToPlay.Points[k];
                        AnimationPointDto rotatedPoint = RotatePoint(point, settingToPlay);

                        await LaserConnectionLogic.SendMessage(new LaserMessage
                        {
                            X = rotatedPoint.X,
                            Y = rotatedPoint.Y,
                            RedLaser = rotatedPoint.RedLaserPowerPwm,
                            GreenLaser = rotatedPoint.GreenLaserPowerPwm,
                            BlueLaser = rotatedPoint.BlueLaserPowerPwm,
                        });

                        if (k == pointsCount - 1)
                        {
                            await LaserConnectionLogic.SendMessage(new LaserMessage
                            {
                                X = rotatedPoint.X,
                                Y = rotatedPoint.Y,
                                RedLaser = 0,
                                GreenLaser = 0,
                                BlueLaser = 0,
                            });
                        }
                    }
                }
            }
        }

        private static AnimationPointDto RotatePoint(AnimationPointDto point, PatternAnimationSettingsDto setting)
        {
            double radians = (Math.PI / 180) * setting.Rotation;
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            int xWithOffset = point.X + setting.CenterX;
            int yWithOffset = point.Y + setting.CenterY;

            int x = (int)(cos * xWithOffset + sin * yWithOffset);
            int y = (int)(cos * yWithOffset - sin * xWithOffset);
            return new AnimationPointDto
            {
                X = x,
                Y = y,
                RedLaserPowerPwm = point.RedLaserPowerPwm,
                GreenLaserPowerPwm = point.GreenLaserPowerPwm,
                BlueLaserPowerPwm = point.BlueLaserPowerPwm,
            };
        }
    }
}
