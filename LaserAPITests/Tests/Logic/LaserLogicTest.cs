using LaserAPI.Logic;
using LaserAPI.Models.Helper.Laser;
using LaserAPI.Models.Helper.Zones;
using LaserAPITests.Mock;
using LaserAPITests.MockedModels.Zones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class LaserLogicTest
    {
        private readonly LaserLogic _laserLogic = new MockLaserLogic().LaserLogic;
        private readonly List<ZonesHitData> _zonesHitData = new MockedZonesHit().ZonesHit;

        [TestMethod]
        public async Task SendDataTest()
        {
            await _laserLogic.SendData(new LaserMessage
            {
                RedLaser = 255,
                BlueLaser = 0,
                GreenLaser = 100,
                X = 0,
                Y = 4000
            });
        }

        [TestMethod]
        public void GetMessagesOnZoneEdgeTest()
        {
            LaserMessage message = new(255, 255, 255, 0, 4000);
            List<ZonesHitData> zonesHit = _zonesHitData;
            ZonesHitData zonesHitData = zonesHit[0];
            zonesHitData.ZoneSidesHit.BottomHit = true;
            zonesHitData.ZoneSidesHit.TopHit = true;

            IReadOnlyList<LaserMessage> messagesOnZonesEdge = LaserLogic.GetMessagesOnZonesEdge(message,
                zonesHit, _zonesHitData.Count);

            Assert.IsTrue(messagesOnZonesEdge.Any());
        }

        [TestMethod]
        public void GetGetMessagesOnZonesEdgeTest()
        {
            IReadOnlyList<LaserMessage> messagesOnZonesEdge = LaserLogic.GetMessagesOnZonesEdge(new LaserMessage(
                255, 255, 255, -4000, 4000), _zonesHitData, _zonesHitData.Count);

            Assert.IsFalse(messagesOnZonesEdge.Any());
        }
    }
}
