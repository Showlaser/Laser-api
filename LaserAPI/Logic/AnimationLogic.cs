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

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < animationDuration)
            {
                List<PatternAnimationDto> patternAnimationsToPlay = PatternAnimationHelper
                    .GetPatternAnimationsBetweenTimeMs(animation, stopwatch.ElapsedMilliseconds);
                if (patternAnimationsToPlay == null)
                {
                    continue;
                }

                PatternAnimationSettingsDto settingToPlay = null;
                foreach (PatternAnimationDto patternAnimation in patternAnimationsToPlay)
                {
                    PatternAnimationSettingsDto closestPatternAnimationSettings = PatternSettingsHelper
                        .GetSettingClosestToTimeMs(patternAnimation.AnimationSettings, patternAnimation.StartTimeOffset,
                            stopwatch.ElapsedMilliseconds);

                    if (closestPatternAnimationSettings != null)
                    {
                        settingToPlay = closestPatternAnimationSettings;
                    }
                }

                if (settingToPlay == null)
                {
                    continue;
                }

                foreach (AnimationPointDto point in settingToPlay.Points)
                {
                    await LaserConnectionLogic.SendMessage(new LaserMessage
                    {
                        X = point.X + settingToPlay.CenterX,
                        Y = point.Y + settingToPlay.CenterY,
                        RedLaser = point.RedLaserPowerPwm,
                        GreenLaser = point.GreenLaserPowerPwm,
                        BlueLaser = point.BlueLaserPowerPwm,
                    });
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
