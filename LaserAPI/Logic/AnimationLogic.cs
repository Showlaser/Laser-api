using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class AnimationLogic(IAnimationDal _animationDal)
    {
        public async Task AddOrUpdate(AnimationDto animation)
        {
            if (!AnimationIsValid(animation))
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

        private static bool AnimationPatternKeyFramesAreValid(AnimationPatternKeyFrameDto keyFrames) =>
            keyFrames.Uuid != Guid.Empty &&
                keyFrames.AnimationPatternUuid != Guid.Empty &&
                keyFrames.TimeMs >= 0 &&
                Enum.IsDefined<PropertyEdited>(keyFrames.PropertyEdited);

        /// <summary>
        /// Method to check if the animation patterns in the animation are valid
        /// </summary>
        /// <param name="animation">The animation</param>
        /// <returns>True if all items in the animationPatterns have a valid value</returns>
        private static bool AnimationPatternsAreValid(AnimationDto animation) =>
            animation.AnimationPatterns.TrueForAll(ap =>
            {
                return ap.Uuid != Guid.Empty &&
                ap.AnimationUuid != Guid.Empty &&
                ap.PatternUuid != Guid.Empty &&
                !string.IsNullOrEmpty(ap.Name) &&
                PatternLogic.PatternIsValid(ap.Pattern) &&
                ap.AnimationPatternKeyFrames.TrueForAll(AnimationPatternKeyFramesAreValid) &&
                ap.StartTimeMs >= 0 &&
                ap.TimelineId.IsBetweenOrEqualTo(0, 3);
            });

        public static bool AnimationIsValid(AnimationDto animation)
        {
            return !string.IsNullOrEmpty(animation.Name) &&
                animation.Uuid != Guid.Empty &&
                !string.IsNullOrEmpty(animation.Image) &&
                AnimationPatternsAreValid(animation);
        }
    }
}
