using LaserAPI.Logic;
using LaserAPI.Models.Helper;
using LaserAPI.Models.Helper.Zones;
using LaserAPITests.Mock;
using LaserAPITests.MockedModels.Zones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class LaserLogicTest
    {
        private readonly LaserLogic _laserLogic = new MockLaserLogic().LaserLogic;
        private readonly List<ZonesHitData> _zonesHitData = new MockedZonesHit().ZonesHit;

        [TestInitialize]
        public void Setup()
        {
            LaserConnectionLogic.RanByUnitTest = true;
        }

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
        public async Task SendDataPerformanceTest()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int iterations = 0;

            while (stopwatch.ElapsedMilliseconds < 1000)
            {
                await _laserLogic.SendData(new LaserMessage
                {
                    RedLaser = 255,
                    BlueLaser = 0,
                    GreenLaser = 100,
                    X = Convert.ToInt32(stopwatch.ElapsedMilliseconds),
                    Y = Convert.ToInt32(stopwatch.ElapsedMilliseconds)
                });

                iterations++;
            }

            stopwatch.Stop();
            Assert.IsTrue(iterations > 25000); // 25000 is for 
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

        [TestMethod]
        public void LimitLaserPowerIfNecessaryTest()
        {
            for (int i = 0; i < 255; i++)
            {
                LaserMessage message = new()
                {
                    RedLaser = 255,
                    GreenLaser = 255,
                    BlueLaser = 255
                };

                LaserLogic.LimitLaserPowerPerLaserIfNecessary(ref message, i);
                Assert.IsTrue(message.RedLaser <= i);
                Assert.IsTrue(message.GreenLaser <= i);
                Assert.IsTrue(message.BlueLaser <= i);
            }
        }

        [TestMethod]
        public void LimitTotalLaserPowerNecessaryTest()
        {
            for (int i = 0; i < 255; i++)
            {
                LaserMessage message = new()
                {
                    RedLaser = 255,
                    GreenLaser = 255,
                    BlueLaser = 255
                };

                LaserLogic.LimitTotalLaserPowerIfNecessary(ref message, i);
                int totalPower = message.RedLaser + message.GreenLaser + message.BlueLaser;
                Assert.IsTrue(totalPower <= i);
            }
        }
    }
}
