using LaserAPI.Logic;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Lasershows;
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
            Assert.IsTrue(durationInMs > 0);
        }

        [TestMethod]
        public void GetLasershowAnimationDuration()
        {
            LasershowAnimationDto lasershowAnimation = new MockedLasershowAnimation().LasershowAnimation;
            int durationInMs = LasershowRenderEngine.GetLasershowAnimationDuration(lasershowAnimation);
            Assert.IsTrue(durationInMs > 0);
        }

        [TestMethod]
        public void GetLasershowAnimationDuration2()
        {
            LasershowAnimationDto lasershowAnimation = new MockedLasershowAnimation().LasershowAnimationList[1];
            int durationInMs = LasershowRenderEngine.GetLasershowAnimationDuration(lasershowAnimation);
            Assert.IsTrue(durationInMs > 100);
        }

        [TestMethod]
        public void GetLasershowDuration()
        {
            LasershowDto lasershow = new MockedLasershow().Lasershow;
            int durationInMs = LasershowRenderEngine.GetLasershowDuration(lasershow);
            Assert.IsTrue(durationInMs > 100);
        }

        [TestMethod]
        public void GetPointsToRender()
        {

        }
    }
}
