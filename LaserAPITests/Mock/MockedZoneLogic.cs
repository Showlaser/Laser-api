using LaserAPI.Interfaces.Dal;
using LaserAPI.Logic;

namespace LaserAPITests.Mock
{
    public class MockedZoneLogic
    {
        public ZoneLogic ZoneLogic { get; }

        public MockedZoneLogic()
        {
            IZoneDal zoneDal = new MockedZoneDal().ZoneDal;
            ZoneLogic = new ZoneLogic(zoneDal);
        }
    }
}
