using LaserAPI.Logic;

namespace LaserAPITests.Mock
{
    internal class MockLaserLogic
    {
        internal LaserLogic LaserLogic;

        public MockLaserLogic()
        {
            LaserLogic mockedLaserLogic = new LaserLogic();
            LaserConnectionLogic.RanByUnitTest = true;

            LaserLogic = mockedLaserLogic;
        }
    }
}
