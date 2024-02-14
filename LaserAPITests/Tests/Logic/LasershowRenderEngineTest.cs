using LaserAPI.Logic;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Lasershows;
using LaserAPITests.MockedModels.Lasershow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class LasershowRenderEngineTest
    {
        /// <summary>
        /// Given the lasershow animation is in the current timeline position
        /// When I execute this method
        /// Then I expect an array of lasershow animations that are within the provided timeline position
        /// </summary>
        [TestMethod]
        public void GetLasershowAnimationInTimelinePositionTest()
        {
            LasershowDto mockedLasershow = new MockedLasershow().Lasershow;
            IEnumerable<LasershowAnimationDto> lasershowAnimations = LasershowRenderEngine.GetLasershowAnimationEqualOrAfterTimelinePosition(mockedLasershow, 0);

            Assert.IsTrue(lasershowAnimations.Any());
        }

        /// <summary>
        /// Given the lasershow animation is in the current timeline position
        /// When
        /// Then
        /// </summary>
        [TestMethod]
        public void GetLasershowAnimationsInTimelinePositionTest()
        {
            LasershowDto mockedLasershow = new MockedLasershow().Lasershow;
            LasershowAnimationDto lasershowAnimation = mockedLasershow.LasershowAnimations.First();
            IEnumerable<AnimationPatternDto> patterns = LasershowRenderEngine
                .GetLasershowAnimationPatternsEqualOrAfterTimelinePosition(lasershowAnimation.Animation.AnimationPatterns, 0);

            Assert.IsTrue(patterns.Any());
        }


        [TestMethod]
        public void GetPointsToRender()
        {

        }
    }
}
