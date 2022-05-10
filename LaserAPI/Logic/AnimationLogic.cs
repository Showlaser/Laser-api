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

        public AnimationLogic(IAnimationDal animationDal)
        {
            _animationDal = animationDal;
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

        public static async Task PlayAnimation(AnimationDto animation)
        {
            int animationDuration = GetAnimationDuration(animation);
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<PatternAnimationSettingsDto> previousPlayedAnimationSettings = new(3);

            while (stopwatch.ElapsedMilliseconds < animationDuration)
            {
                List<PatternAnimationDto> patternAnimationsToPlay =
                    GetPatternAnimationsBetweenTimeMs(animation, stopwatch.ElapsedMilliseconds);
                if (patternAnimationsToPlay == null)
                {
                    continue;
                }

                List<PatternAnimationSettingsDto> settingsToPlay = new(3);
                int patternAnimationsLength = patternAnimationsToPlay.Count;

                int duration = 0;
                GetPatternAnimationSettingsToPlay(patternAnimationsLength, patternAnimationsToPlay,
                    stopwatch.ElapsedMilliseconds, ref settingsToPlay, ref duration);

                if (settingsToPlay.Count == 0 || !LaserConnectionLogic.LaserIsAvailable())
                {
                    continue;
                }

                if (PreviousSettingsEqualNewSettings(previousPlayedAnimationSettings, settingsToPlay) && previousPlayedAnimationSettings.Count > 0)
                {
                    continue;
                }

                List<LaserMessage> messagesToPlay = GetAnimationPointsToPlay(settingsToPlay);
                await LaserLogic.SendData(messagesToPlay, duration);
                previousPlayedAnimationSettings.Clear();
                previousPlayedAnimationSettings.AddRange(settingsToPlay);
            }
        }

        private static bool PreviousSettingsEqualNewSettings(
            IReadOnlyList<PatternAnimationSettingsDto> previousSettings,
            IReadOnlyList<PatternAnimationSettingsDto> newSettings)
        {
            int previousSettingsLength = previousSettings.Count;
            int newSettingsLength = newSettings.Count;
            if (previousSettingsLength != newSettingsLength)
            {
                return true;
            }

            for (int i = 0; i < previousSettingsLength; i++)
            {
                if (previousSettings[i].Uuid != newSettings[i].Uuid)
                {
                    return false;
                }
            }

            return true;
        }

        private static List<LaserMessage> GetAnimationPointsToPlay(
            IReadOnlyList<PatternAnimationSettingsDto> settingsToPlay)
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

        private static void GetPatternAnimationSettingsToPlay(int patternAnimationsLength,
            IReadOnlyList<PatternAnimationDto> patternAnimationsToPlay,
            long stopwatchTime, ref List<PatternAnimationSettingsDto> settingsToPlay, ref int duration)
        {
            for (int i = 0; i < patternAnimationsLength; i++)
            {
                PatternAnimationDto patternAnimation = patternAnimationsToPlay[i];
                PatternAnimationSettingsDto closestPatternAnimationSettings = GetSettingClosestToTimeMs(
                    patternAnimation.AnimationSettings, patternAnimation.StartTimeOffset,
                    stopwatchTime, ref duration);

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
        /// Gets the pattern animations the fall between the stopwatchTime value
        /// </summary>
        /// <param name="animation">The animation to search the pattern animations in</param>
        /// <param name="timeMs">The time that the pattern animations need to fall between</param>
        /// <returns>The pattern animations that matches the time, sorted by time</returns>
        public static List<PatternAnimationDto> GetPatternAnimationsBetweenTimeMs(AnimationDto animation, long timeMs)
        {
            int patternAnimationsCount = animation.PatternAnimations.Count;
            List<PatternAnimationDto> patternAnimations = new(patternAnimationsCount);
            for (int i = 0; i < patternAnimationsCount; i++)
            {
                PatternAnimationDto pa = animation.PatternAnimations[i];
                int animationSettingsLength = pa.AnimationSettings.Count;
                int startTimeOffset = pa.StartTimeOffset;
                int min = 0;
                int max = 0;

                GetHighestAndLowestStartTimeFromSettings(pa.AnimationSettings, animationSettingsLength, ref min, ref max);
                if (timeMs.IsBetweenOrEqualTo(min + startTimeOffset, max + startTimeOffset))
                {
                    patternAnimations.Add(pa);
                }
            }

            int[] patternAnimationsStartTimeOffsetCollection = new int[patternAnimationsCount];
            for (int i = 0; i < patternAnimationsCount; i++)
            {
                patternAnimationsStartTimeOffsetCollection[i] = patternAnimations[i].StartTimeOffset;
            }

            QuickSortHelper.QuickSort(patternAnimationsStartTimeOffsetCollection);
            List<PatternAnimationDto> sortedPatternAnimations = new(patternAnimationsCount);

            int index = 0;
            while (patternAnimations.Count > 0)
            {
                PatternAnimationDto patternAnimation = patternAnimations[index];
                if (patternAnimation.StartTimeOffset == patternAnimationsStartTimeOffsetCollection[index])
                {
                    patternAnimations.RemoveAt(index);
                    sortedPatternAnimations.Add(patternAnimation);
                }

                index++;
                if (index > patternAnimations.Count - 1)
                {
                    index = 0;
                }
            }

            return sortedPatternAnimations;
        }

        private static void GetHighestAndLowestStartTimeFromSettings(IReadOnlyList<PatternAnimationSettingsDto> settings,
            int settingsLength, ref int lowest, ref int highest)
        {
            int lowestStartTime = settings[0].StartTime;
            int highestStartTime = settings[0].StartTime;
            for (int i = 0; i < settingsLength; i++)
            {
                int startTime = settings[i].StartTime;
                if (startTime < lowestStartTime)
                {
                    lowestStartTime = startTime;
                }

                if (startTime > highestStartTime)
                {
                    highestStartTime = startTime;
                }
            }

            highest = highestStartTime;
            lowest = lowestStartTime;
        }

        public static PatternAnimationSettingsDto GetSettingClosestToTimeMs(
            List<PatternAnimationSettingsDto> settings, int offsetTime, long stopwatchTime, ref int duration)
        {
            int settingsLength = settings.Count;
            if (settingsLength == 0)
            {
                return null;
            }

            if (settingsLength == 1)
            {
                return settings[0];
            }

            int lowestStartTime = settings[0].StartTime;
            int lowestStartTimeIndex = 0;

            for (int i = 1; i < settingsLength; i++)
            {
                PatternAnimationSettingsDto setting = settings[i];
                bool startTimeIsUnderStopwatchTime = setting.StartTime + offsetTime < stopwatchTime;
                bool startTimeIsCurrentlyLowest = lowestStartTime > setting.StartTime;
                if (startTimeIsUnderStopwatchTime && startTimeIsCurrentlyLowest)
                {
                    lowestStartTime = setting.StartTime;
                    lowestStartTimeIndex = i;
                }

                if (i == settingsLength - 1)
                {
                    continue;
                }

                PatternAnimationSettingsDto nextSetting = settings[i + 1];
                int settingDuration = nextSetting.StartTime - setting.StartTime;
                if (settingDuration < duration)
                {
                    duration = settingDuration;
                }
            }

            return settings[lowestStartTimeIndex];
        }
    }
}
