using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper.Zones;

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
