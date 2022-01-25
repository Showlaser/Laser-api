using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Data;
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
            return points.Any() && points.TrueForAll(p => p.TimelineSettingsUuid != Guid.Empty);
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

        private static void ValidateAnimation(AnimationDto animation)
        {
            bool animationValid = animation != null &&
                                  SettingsValid(animation.PatternAnimations
                                      .Select(p => p.AnimationSettings)
                                      .ToList()) &&
                                PointsValid(animation.PatternAnimations
                                    .SelectMany(p => p.AnimationSettings.Points)
                                    .ToList()) &&
                                  PatternAnimationValid(animation.PatternAnimations);
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

        public async Task Update(AnimationDto animation)
        {
            ValidateAnimation(animation);
            await _animationDal.Update(animation);
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
