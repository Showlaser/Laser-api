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
            if (!AnimationPatternsAreValid(animation))
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

            while (stopwatch.ElapsedMilliseconds < animationDuration)
            {
                List<AnimationPatternDto> patternAnimationsToPlay =
                    GetPatternAnimationsBetweenTimeMs(animation, stopwatch.ElapsedMilliseconds);
                if (patternAnimationsToPlay == null)
                {
                    continue;
                }

                List<PatternAnimationSettingsDto> settingsToPlay = new(3);
                int patternAnimationsLength = patternAnimationsToPlay.Count;

                GetPatternAnimationSettingsToPlay(patternAnimationsLength, patternAnimationsToPlay,
                    stopwatch.ElapsedMilliseconds, ref settingsToPlay);

                if (settingsToPlay.Count == 0 || !LaserConnectionLogic.LaserIsAvailable())
                {
                    continue;
                }

                List<LaserMessage> messagesToPlay = GetAnimationPointsToPlay(settingsToPlay);
                int duration = GetDurationFromSettings(settingsToPlay, patternAnimationsToPlay.SelectMany(pa => pa.AnimationSettings).ToList());
                await LaserLogic.SendData(messagesToPlay, duration);
            }
        }

        private static bool AnimationPatternKeyFramesAreValid(AnimationPatternKeyFrameDto keyFrames) =>
            keyFrames.Uuid != Guid.Empty &&
                keyFrames.AnimationPatternUuid != Guid.Empty &&
                keyFrames.TimeMs >= 0 &&
                !string.IsNullOrEmpty(keyFrames.PropertyEdited);

        /// <summary>
        /// Method to check if the animation patterns in the animation are valid
        /// </summary>
        /// <param name="animation">The animation</param>
        /// <returns>True if all items in the animationPatterns have a valid value</returns>
        public static bool AnimationPatternsAreValid(AnimationDto animation) =>
            animation.AnimationPatterns.TrueForAll(ap =>
            {
                return ap.Uuid != Guid.Empty &&
                ap.AnimationUuid != Guid.Empty &&
                !string.IsNullOrEmpty(ap.Name) &&
                PatternLogic.PatternIsValid(ap.Pattern) &&
                ap.AnimationPatternKeyFrames.TrueForAll(AnimationPatternKeyFramesAreValid) &&
                ap.StartTimeMs >= 0 &&
                ap.TimeLineId.IsBetweenOrEqualTo(0, 3);
            });
    }
}
