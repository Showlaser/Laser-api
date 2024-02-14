using LaserAPI.Logic;

namespace LaserAPITests.Mock
{
    internal class MockedLaserLogic
    {
        internal LaserLogic LaserLogic;

        public MockedLaserLogic()
        {
            MockedZoneLogic mockedZoneLogic = new();
            LaserLogic mockedLaserLogic = new(mockedZoneLogic.ZoneLogic);

            LaserLogic = mockedLaserLogic;
        }
    }
}
