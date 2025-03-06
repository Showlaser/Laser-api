using LaserAPI.Logic;
using LaserAPI.Models.Dto.Animations;
using LaserAPITests.Mock;
using LaserAPITests.MockedModels.Animation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            AnimationLogic.AnimationIsValid(animation);
        }
    }
}
