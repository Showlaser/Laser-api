using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Lasershows;
using LaserAPI.Models.Dto.Patterns;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Logic
{
    public static class LasershowRenderEngine
    {
        public static IEnumerable<LasershowAnimationDto> GetLasershowAnimationsInTimelinePosition(LasershowDto lasershow, int timelinePosition)
        {
            return lasershow.LasershowAnimations
                .Where(la => NumberIsBetweenOrEqual(timelinePosition, la.StartTimeMs, GetLasershowAnimationDuration(la) + la.StartTimeMs));
        }

        public static IEnumerable<AnimationPatternDto> GetAnimationPatternsEqualOrAfterTimelinePosition(IEnumerable<LasershowAnimationDto> lasershowAnimationPatterns,
            int timelinePosition)
        {
            return lasershowAnimationPatterns.SelectMany(lsap => lsap.Animation.AnimationPatterns
                                             .Where(ap => NumberIsBetweenOrEqual(timelinePosition, ap.StartTimeMs, GetAnimationPatternDuration(ap) + ap.StartTimeMs)));
        }

        public static int GetAnimationPatternDuration(AnimationPatternDto animationPattern)
        {
            return animationPattern.AnimationPatternKeyFrames.MaxBy(apkf => apkf.TimeMs).TimeMs + animationPattern.StartTimeMs;
        }

        public static int GetLasershowAnimationDuration(LasershowAnimationDto lasershowAnimation)
        {
            return lasershowAnimation.Animation.AnimationPatterns
                .Max(ap => ap.StartTimeMs + GetAnimationPatternDuration(ap)) + lasershowAnimation.StartTimeMs;
        }

        public static int GetLasershowDuration(LasershowDto lasershow)
        {
            IEnumerable<int> animationDurations = lasershow.LasershowAnimations.Select(ls => ls.StartTimeMs + GetLasershowAnimationDuration(ls));
            return animationDurations.Max();
        }

        private static List<AnimationPatternKeyFrameDto> GetKeyFramesBeforeTimelinePositionSortedByTimeDescending(PropertyEdited property, AnimationPatternDto animationPattern, int TimelinePositionMs)
        {
            List<AnimationPatternKeyFrameDto> aPKF = animationPattern.AnimationPatternKeyFrames
                .FindAll(ak => ak.TimeMs < TimelinePositionMs && ak.PropertyEdited == property);

            aPKF.Sort((a, b) => b.TimeMs - a.TimeMs);
            return aPKF;
        }

        private static List<AnimationPatternKeyFrameDto> GetKeyFramesPastTimelinePositionSortedByTime(PropertyEdited property, AnimationPatternDto animationPattern, int TimelinePositionMs)
        {
            List<AnimationPatternKeyFrameDto> aPKF = animationPattern.AnimationPatternKeyFrames
                .FindAll(ak => ak.TimeMs > TimelinePositionMs && ak.PropertyEdited == property);

            aPKF.Sort((a, b) => a.TimeMs - b.TimeMs);
            return aPKF;
        }

        public static PreviousCurrentAndNextKeyFramePerProperty GetPreviousCurrentAndNextKeyFramePerProperty(AnimationPatternDto animationPatternDto, int TimelinePositionMs)
        {
            PreviousCurrentAndNextKeyFramePerProperty previousNextAndCurrentKeyFramePerProperty = new()
            {
                Previous = [],
                Current = animationPatternDto.AnimationPatternKeyFrames.FindAll(ak => ak.TimeMs == TimelinePositionMs),
                Next = []
            };

            foreach (PropertyEdited propertyEdited in Enum.GetValues(typeof(PropertyEdited)))
            {
                AnimationPatternKeyFrameDto previous = GetKeyFramesBeforeTimelinePositionSortedByTimeDescending(
                    propertyEdited, animationPatternDto, TimelinePositionMs).FirstOrDefault();
                if (previous != null)
                {
                    previousNextAndCurrentKeyFramePerProperty.Previous.Add(previous);
                }

                AnimationPatternKeyFrameDto next = GetKeyFramesPastTimelinePositionSortedByTime(
                    propertyEdited, animationPatternDto, TimelinePositionMs).FirstOrDefault();
                if (next != null)
                {
                    previousNextAndCurrentKeyFramePerProperty.Next.Add(next);
                }
            }

            return previousNextAndCurrentKeyFramePerProperty;
        }

        public static IEnumerable<PointDto> GetPointsToRender(LasershowDto lasershow, int timelinePosition)
        {
            IEnumerable<LasershowAnimationDto> lasershowAnimations = GetLasershowAnimationsInTimelinePosition(lasershow, timelinePosition);
            IEnumerable<AnimationPatternDto> animationPatternsToPlay = GetAnimationPatternsEqualOrAfterTimelinePosition(lasershowAnimations, timelinePosition);

            List<PointDto> pointsToRender = new();
            foreach (AnimationPatternDto pattern in animationPatternsToPlay)
            {
                //pattern.
            }

            return [];
        }

        public static bool NumberIsBetweenOrEqual(int number, int min, int max)
        {
            return number >= min && number <= max;
        }
    }
}
