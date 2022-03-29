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
        private readonly LaserLogic _laserLogic;

        public AnimationLogic(IAnimationDal animationDal, LaserLogic laserLogic)
        {
            _animationDal = animationDal;
            _laserLogic = laserLogic;
        }

        public async Task AddOrUpdate(AnimationDto animation)
        {
            if (!AnimationHelper.AnimationValid(animation))
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

                List<PatternAnimationSettingsDto> settingsToPlay = new();
                int patternAnimationsLength = patternAnimationsToPlay.Count;

                long stopwatchTime = stopwatch.ElapsedMilliseconds;
                GetPatternAnimationSettingsToPlay(patternAnimationsLength, patternAnimationsToPlay,
                    stopwatchTime, ref settingsToPlay);

                if (!settingsToPlay.Any())
                {
                    continue;
                }

                int settingsToPlayLength = settingsToPlay.Count;
                for (int i = 0; i < settingsToPlayLength; i++)
                {
                    PatternAnimationSettingsDto settingToPlay = settingsToPlay[i];
                    settingToPlay.Points = settingToPlay.Points.OrderBy(p => p.Order).ToList();
                    await PlayAnimationPoints(settingToPlay);
                }
            }
        }

        private async Task PlayAnimationPoints(PatternAnimationSettingsDto settingToPlay)
        {
            int pointsCount = settingToPlay.Points.Count;
            for (int k = 0; k < pointsCount; k++)
            {
                AnimationPointDto point = settingToPlay.Points[k];
                AnimationPointDto rotatedPoint = RotatePoint(point, settingToPlay);

                await _laserLogic.SendData(new LaserMessage
                {
                    X = rotatedPoint.X,
                    Y = rotatedPoint.Y,
                    RedLaser = rotatedPoint.RedLaserPowerPwm,
                    GreenLaser = rotatedPoint.GreenLaserPowerPwm,
                    BlueLaser = rotatedPoint.BlueLaserPowerPwm,
                });

                if (k == pointsCount - 1)
                {
                    await _laserLogic.SendData(new LaserMessage
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

        private static void GetPatternAnimationSettingsToPlay(int patternAnimationsLength, IReadOnlyList<PatternAnimationDto> patternAnimationsToPlay,
            long stopwatchTime, ref List<PatternAnimationSettingsDto> settingsToPlay)
        {
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
