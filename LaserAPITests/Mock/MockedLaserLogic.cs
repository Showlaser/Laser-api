using LaserAPI.Logic;

namespace LaserAPITests.Mock
{
    internal class MockedLaserLogic
    {
        internal LaserLogic LaserLogic;

        public MockedLaserLogic()
        {
            MockedZoneLogic mockedZoneLogic = new();
            MockedLaserConnectionLogic mockedLaserConnectionLogic = new();
            LaserLogic mockedLaserLogic = new(mockedZoneLogic.ZoneLogic, mockedLaserConnectionLogic.LaserConnectionLogic);

            LaserLogic = mockedLaserLogic;
        }
    }
}
