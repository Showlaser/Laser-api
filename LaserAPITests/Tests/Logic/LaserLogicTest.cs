using System.Collections.Generic;
using System.Linq;
using LaserAPI.Logic;
using LaserAPI.Models.Helper.Laser;
using LaserAPITests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using LaserAPI.Models.Helper.Zones;
using LaserAPITests.MockedModels.Zones;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class LaserLogicTest
    {
        private static readonly LaserLogic LaserLogic = new MockLaserLogic().LaserLogic;
        private static readonly List<ZonesHitData> ZonesHitData = new MockedZonesHit().ZonesHit;

        [TestMethod]
        public async Task SendDataTest()
        {
            await LaserLogic.SendData(new LaserMessage
            {
                RedLaser = 255,
                BlueLaser = 0,
                GreenLaser = 100,
                X = 0,
                Y = 4000
            });
        }

        [TestMethod]
        public void GetSortedMessagesToSendOutsideZoneTest()
        {
            IReadOnlyList<LaserMessage> sortedMessages = LaserLogic.GetMessagesOnZonesEdge(new LaserMessage(
                255, 255, 255, -4000, 4000), ZonesHitData, ZonesHitData.Count);

            LaserMessage message = sortedMessages.First();
            Assert.IsTrue(message.X == -4000);
            Assert.IsTrue(message.Y == 4000);
            Assert.IsTrue(message.RedLaser == 255);
            Assert.IsTrue(message.GreenLaser == 255);
            Assert.IsTrue(message.BlueLaser == 255);
        }
    }
}
