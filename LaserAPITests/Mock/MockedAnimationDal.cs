using LaserAPI.Interfaces.Dal;
using LaserAPITests.MockedModels.Animation;
using Moq;

namespace LaserAPITests.Mock
{
    internal class MockedAnimationDal
    {
        public readonly IAnimationDal AnimationDal;

        public MockedAnimationDal()
        {
            MockedAnimation mockedAnimation = new();
            Mock<IAnimationDal> mockedAnimationDal = new();
            mockedAnimationDal.Setup(d => d.All()).ReturnsAsync(mockedAnimation.AnimationList);
            AnimationDal = mockedAnimationDal.Object;
        }
    }
}
