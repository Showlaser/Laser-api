using LaserAPI.Logic;

namespace LaserAPITests.Mock
{
    internal class MockLaserLogic
    {
        internal LaserLogic LaserLogic;

        public MockLaserLogic()
        {
            MockedZoneLogic mockedZoneLogic = new();
            LaserLogic mockedLaserLogic = new(mockedZoneLogic.ZoneLogic);
            LaserConnectionLogic.RanByUnitTest = true;

            LaserLogic = mockedLaserLogic;
        }
    }
}
