using LaserAPI.Logic;

namespace LaserAPITests.Mock
{
    internal class MockedAnimationLogic
    {
        internal AnimationLogic AnimationLogic;

        public MockedAnimationLogic()
        {
            MockedAnimationDal mockedAnimationDal = new();
            AnimationLogic mockedAnimationLogic = new(mockedAnimationDal.AnimationDal);

            AnimationLogic = mockedAnimationLogic;
        }
    }
}
