using LaserAPI.Models.Helper.Zones;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPITests.MockedModels.Zones
{
    public class MockedZonesHit
    {
        public readonly List<ZonesHitData> ZonesHit;

        public MockedZonesHit()
        {
            ZonesHitData zoneHitData = new()
            {
                Zone = new MockedZones().Zones.First()
            };

            zoneHitData.ZoneAbsolutePositions = new ZoneAbsolutePositionsHelper(zoneHitData.Zone);
            zoneHitData.ZoneSidesHit = new ZoneSidesHitHelper();

            ZonesHit = new List<ZonesHitData> { zoneHitData };
        }
    }
}
