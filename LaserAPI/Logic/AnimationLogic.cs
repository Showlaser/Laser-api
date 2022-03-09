using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Helper;
using LaserAPI.Models.Helper.Laser;
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

        public async Task PlayAnimation(AnimationDto animation)
        {
            int animationDuration = AnimationHelper.GetAnimationDuration(animation);
            int iterations = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < animationDuration)
            {
                iterations++;
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
                        await LaserConnectionLogic.SendMessage(new LaserMessage
                        {
                            X = point.X + settingToPlay.CenterX,
                            Y = point.Y + settingToPlay.CenterY,
                            RedLaser = point.RedLaserPowerPwm,
                            GreenLaser = point.GreenLaserPowerPwm,
                            BlueLaser = point.BlueLaserPowerPwm,
                        });

                        if (k == pointsCount - 1)
                        {
                            await LaserConnectionLogic.SendMessage(new LaserMessage
                            {
                                X = point.X + settingToPlay.CenterX,
                                Y = point.Y + settingToPlay.CenterY,
                                RedLaser = 0,
                                GreenLaser = 0,
                                BlueLaser = 0,
                            });
                        }
                    }
                }
            }
        }

        private static bool AnimationDoesNotContainsSettingsWithSameStartTime(AnimationDto animation)
        {
            List<int> startTimeCollection = animation.PatternAnimations
                .SelectMany(pa => pa.AnimationSettings
                    .Select(ase => ase.StartTime))
                .ToList();

            return startTimeCollection.Distinct().Any();
        }

        private static void ValidateAnimation(AnimationDto animation)
        {
            bool animationValid = animation != null &&
                                  animation.PatternAnimations
                                      .TrueForAll(pa => pa.AnimationSettings
                                          .TrueForAll(ase => SettingsValid(pa.AnimationSettings))) &&
                                  animation.PatternAnimations
                                      .TrueForAll(pa => pa.AnimationSettings
                                          .TrueForAll(ase => PointsValid(ase.Points))) &&
                                  PatternAnimationValid(animation.PatternAnimations) &&
                                  AnimationDoesNotContainsSettingsWithSameStartTime(animation);
            if (!animationValid)
            {
                throw new InvalidDataException(nameof(animation));
            };
        }

        public async Task AddOrUpdate(AnimationDto animation)
        {
            ValidateAnimation(animation);
            if (await _animationDal.Exists(animation.Uuid))
            {
                await _animationDal.Update(animation);
                return;
            }

            await _animationDal.Add(animation);
        }

        public async Task<List<AnimationDto>> All()
        {
            return await _animationDal.All();
        }

        public async Task Remove(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                throw new NoNullAllowedException(nameof(uuid));
            }
            await _animationDal.Remove(uuid);
        }
    }
}
