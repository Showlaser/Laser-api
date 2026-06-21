using LaserAPI.Logic;
using LaserAPI.Models.Dto.Animations;
using LaserAPITests.Mock;
using LaserAPITests.MockedModels.Animation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class AnimationLogicTest
    {
        private readonly AnimationLogic _animationLogic = new MockedAnimationLogic().AnimationLogic;

        [TestMethod]
        public void AnimationIsValidTest()
        {
            AnimationDto animation = new MockedAnimation().Animation;
            Assert.IsTrue(AnimationLogic.AnimationIsValid(animation));
        }

        [TestMethod]
        public void AnimationIsInvalidWhenNameIsEmptyTest()
        {
            AnimationDto animation = new MockedAnimation().Animation;
            animation.Name = "";
            Assert.IsFalse(AnimationLogic.AnimationIsValid(animation));
        }

        [TestMethod]
        public void AnimationIsInvalidWhenImageIsEmptyTest()
        {
            AnimationDto animation = new MockedAnimation().Animation;
            animation.Image = "";
            Assert.IsFalse(AnimationLogic.AnimationIsValid(animation));
        }

        [TestMethod]
        public void AnimationIsInvalidWhenUuidIsEmptyTest()
        {
            AnimationDto animation = new MockedAnimation().Animation;
            animation.Uuid = Guid.Empty;
            Assert.IsFalse(AnimationLogic.AnimationIsValid(animation));
        }

        [TestMethod]
        public void AnimationIsInvalidWhenTimelineIdOutOfRangeTest()
        {
            AnimationDto animation = new MockedAnimation().Animation;
            animation.AnimationPatterns[0].TimelineId = 4;
            Assert.IsFalse(AnimationLogic.AnimationIsValid(animation));
        }

        [TestMethod]
        public void AnimationIsInvalidWhenPatternUuidIsEmptyTest()
        {
            AnimationDto animation = new MockedAnimation().Animation;
            animation.AnimationPatterns[0].PatternUuid = Guid.Empty;
            Assert.IsFalse(AnimationLogic.AnimationIsValid(animation));
        }

        [TestMethod]
        public void AnimationIsInvalidWhenStartTimeIsNegativeTest()
        {
            AnimationDto animation = new MockedAnimation().Animation;
            animation.AnimationPatterns[0].StartTimeMs = -1;
            Assert.IsFalse(AnimationLogic.AnimationIsValid(animation));
        }

        [TestMethod]
        public async Task AddOrUpdateWithValidAnimationSucceedsTest()
        {
            await _animationLogic.AddOrUpdate(new MockedAnimation().Animation);
        }

        [TestMethod]
        public async Task AddOrUpdateWithInvalidAnimationThrowsTest()
        {
            await Assert.ThrowsAsync<InvalidDataException>(
                async () => await _animationLogic.AddOrUpdate(new MockedAnimation().Empty));
        }

        [TestMethod]
        public async Task RemoveWithEmptyGuidThrowsTest()
        {
            await Assert.ThrowsAsync<NoNullAllowedException>(async () => await _animationLogic.Remove(Guid.Empty));
        }
    }
}
