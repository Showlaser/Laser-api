using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class AnimationLogic
    {
        private readonly IAnimationDal _animationDal;
        private readonly LaserLogic _laserLogic;

        public AnimationLogic(IAnimationDal animationDal, LaserLogic laserLogic)
        {
            _animationDal = animationDal;
            _laserLogic = laserLogic;
        }

        public async Task AddOrUpdate(AnimationDto animation)
        {
            if (!AnimationValid(animation))
            {
                throw new InvalidDataException();
            }
            if (await _animationDal.Exists(animation.Uuid))
            {
                await _animationDal.Update(animation);
                return;
            }

            await _animationDal.Add(animation);
        }

        public async Task<List<AnimationDto>> All()
        {
            List<AnimationDto> data = await _animationDal.All();
            return data;
        }

        public async Task Remove(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                throw new NoNullAllowedException(nameof(uuid));
            }
            await _animationDal.Remove(uuid);
        }

        public async Task PlayAnimation(AnimationDto animation)
        {
            int animationDuration = GetAnimationDuration(animation);
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (stopwatch.ElapsedMilliseconds < animationDuration)
            {
                List<PatternAnimationDto> patternAnimationsToPlay = GetPatternAnimationsBetweenTimeMs(animation, stopwatch.ElapsedMilliseconds);
                if (patternAnimationsToPlay == null)
                {
                    continue;
                }

                List<PatternAnimationSettingsDto> settingsToPlay = new();
                int patternAnimationsLength = patternAnimationsToPlay.Count;

                long stopwatchTime = stopwatch.ElapsedMilliseconds;
                GetPatternAnimationSettingsToPlay(patternAnimationsLength, patternAnimationsToPlay,
                    stopwatchTime, ref settingsToPlay);

                if (!settingsToPlay.Any())
                {
                    continue;
                }

                List<LaserMessage> messagesToPlay = GetAnimationPointsToPlay(settingsToPlay);
                await _laserLogic.SendData(messagesToPlay);
            }
        }

        private static List<LaserMessage> GetAnimationPointsToPlay(IReadOnlyList<PatternAnimationSettingsDto> settingsToPlay)
        {
            List<LaserMessage> messagesToPlay = new();
            int settingsToPlayLength = settingsToPlay.Count;

            for (int i = 0; i < settingsToPlayLength; i++)
            {
                PatternAnimationSettingsDto settingToPlay = settingsToPlay[i];
                settingToPlay.Points = settingToPlay.Points.OrderBy(p => p.Order).ToList();

                int pointsCount = settingToPlay.Points.Count;
                for (int k = 0; k < pointsCount; k++)
                {
                    AnimationPointDto point = settingToPlay.Points[k];
                    AnimationPointDto rotatedPoint = RotatePoint(point, settingToPlay);

                    messagesToPlay.Add(new LaserMessage
                    {
                        X = rotatedPoint.X,
                        Y = rotatedPoint.Y,
                        RedLaser = rotatedPoint.RedLaserPowerPwm,
                        GreenLaser = rotatedPoint.GreenLaserPowerPwm,
                        BlueLaser = rotatedPoint.BlueLaserPowerPwm,
                    });
                }
            }

            return messagesToPlay;
        }

        private static void GetPatternAnimationSettingsToPlay(int patternAnimationsLength, IReadOnlyList<PatternAnimationDto> patternAnimationsToPlay,
            long stopwatchTime, ref List<PatternAnimationSettingsDto> settingsToPlay)
        {
            for (int i = 0; i < patternAnimationsLength; i++)
            {
                PatternAnimationDto patternAnimation = patternAnimationsToPlay[i];
                PatternAnimationSettingsDto closestPatternAnimationSettings = GetSettingClosestToTimeMs(patternAnimation.AnimationSettings, patternAnimation.StartTimeOffset,
                        stopwatchTime);

                if (closestPatternAnimationSettings != null)
                {
                    settingsToPlay.Add(closestPatternAnimationSettings);
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
                                                          p.RedLaserPowerPwm.IsBetweenOrEqualTo(0, 255) &&
                                                          p.GreenLaserPowerPwm.IsBetweenOrEqualTo(0, 255) &&
                                                          p.BlueLaserPowerPwm.IsBetweenOrEqualTo(0, 255));
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

        public static PatternAnimationSettingsDto GetSettingClosestToTimeMs(
            List<PatternAnimationSettingsDto> settings, int offsetTime, long timeMs)
        {
            List<PatternAnimationSettingsDto> settingsUnderTimeMs = settings.FindAll(s => s.StartTime + offsetTime < timeMs);
            return settingsUnderTimeMs.OrderByDescending(s => s.StartTime).FirstOrDefault();
        }
    }
}
