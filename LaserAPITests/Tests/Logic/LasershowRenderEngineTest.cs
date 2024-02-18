using LaserAPI.Logic;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Lasershows;
using LaserAPITests.MockedModels.AnimationPatterns;
using LaserAPITests.MockedModels.Lasershow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class LasershowRenderEngineTest
    {
        [TestMethod]
        public void GetAnimationPatternDuration()
        {
            LasershowAnimationDto lasershowAnimation = new MockedLasershowAnimation().LasershowAnimation;
            AnimationPatternDto animationPattern = lasershowAnimation.Animation.AnimationPatterns.First();
            int durationInMs = LasershowRenderEngine.GetAnimationPatternDuration(animationPattern);
            Assert.IsTrue(durationInMs == 50);
        }

        [TestMethod]
        public void GetLasershowAnimationDuration()
        {
            LasershowAnimationDto lasershowAnimation = new MockedLasershowAnimation().LasershowAnimation;
            int durationInMs = LasershowRenderEngine.GetLasershowAnimationDuration(lasershowAnimation);
            Assert.IsTrue(durationInMs == 50);
        }

        [TestMethod]
        public void GetLasershowAnimationDuration2()
        {
            LasershowAnimationDto lasershowAnimation = new MockedLasershowAnimation().LasershowAnimationList[1];
            int durationInMs = LasershowRenderEngine.GetLasershowAnimationDuration(lasershowAnimation);
            Assert.IsTrue(durationInMs == 150);
        }

        [TestMethod]
        public void GetLasershowDuration()
        {
            LasershowDto lasershow = new MockedLasershow().Lasershow;
            int durationInMs = LasershowRenderEngine.GetLasershowDuration(lasershow);
            Assert.IsTrue(durationInMs == 250);
        }

        [TestMethod]
        public void GetPointsToRender()
        {

        }

        [TestMethod]
        public void NumberIsBetweenOrEqual()
        {
            bool isBetweenOrEqual = LasershowRenderEngine.NumberIsBetweenOrEqual(100, 0, 150);
            Assert.IsTrue(isBetweenOrEqual);
        }

        [TestMethod]
        [DataRow(4, 0, 4, 25)]
        [DataRow(0, 4, 4, 0)]
        [DataRow(4, 0, 0, 1000)]
        public void GetPreviousCurrentAndNextKeyFramePerProperty(
            int previousCount,
            int currentCount,
            int nextCount,
            int timelinePositionMs)
        {
            AnimationPatternDto animationPattern = new MockedAnimationPattern().AnimationPattern.First();
            LaserAPI.Models.Helper.PreviousCurrentAndNextKeyFramePerProperty keyFrames = LasershowRenderEngine
                .GetPreviousCurrentAndNextKeyFramePerProperty(animationPattern, timelinePositionMs);

            Assert.IsTrue(keyFrames.Previous.Count == previousCount);
            Assert.IsTrue(keyFrames.Current.Count == currentCount);
            Assert.IsTrue(keyFrames.Next.Count == nextCount);
        }
    }
}
