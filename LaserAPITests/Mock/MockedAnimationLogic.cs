using LaserAPI.Logic;

namespace LaserAPITests.Mock
{
    internal class MockedAnimationLogic
    {
        internal AnimationLogic AnimationLogic;

        public MockedAnimationLogic()
        {
            MockedLaserLogic mockedLaserLogic = new();
            MockedAnimationDal mockedAnimationDal = new();
            AnimationLogic mockedAnimationLogic = new(mockedAnimationDal.AnimationDal, mockedLaserLogic.LaserLogic);

            AnimationLogic = mockedAnimationLogic;
        }
    }
}
