using LaserAPI.Models.Helper.Laser;
using LaserAPI.Models.Helper.Zones;
using LaserAPITests.MockedModels.Zones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPITests.Tests.Helper
{
    [TestClass]
    public class ZoneSideHitHelperTest
    {
        private readonly ZonesHitData _zonesHitData = new MockedZonesHit().ZonesHit.First();

        [TestMethod]
        public void ZoneSideHitTest()
        {
            LaserMessage message = new(255, 255, 255, 0, 4000);

            ZoneSidesHitHelper zoneSideHitHelper = new();
            List<LaserMessage> sortedMessages = new();
            ZonesHitData zonesHitData = _zonesHitData;
            zonesHitData.ZoneSidesHit.BottomHit = true;
            zonesHitData.ZoneSidesHit.TopHit = true;

            zoneSideHitHelper.GetMissingXOrYCoordinateOfZoneCrossing(message, _zonesHitData, 0, -4000, ref sortedMessages);
        }
    }
}
