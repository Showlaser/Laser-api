using LaserAPI.Logic;
using LaserAPI.Models.Helper;
using LaserAPITests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class LaserLogicTest
    {
        private readonly LaserLogic _laserLogic = new MockLaserLogic().LaserLogic;

        [TestInitialize]
        public void Setup()
        {
            LaserConnectionLogic.RanByUnitTest = true;
        }

        [TestMethod]
        public async Task SendDataTest()
        {
            LaserConnectionLogic.PreviousMessage = new LaserMessage(0, 0, 0, -4000, 4000);
            await _laserLogic.SendData(new List<LaserMessage> { new LaserMessage
            {
                RedLaser = 255,
                BlueLaser = 0,
                GreenLaser = 100,
                X = 4000,
                Y = 0
            }});
        }

        [TestMethod]
        public async Task SendDataPerformanceTest()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int iterations = 0;

            while (stopwatch.ElapsedMilliseconds < 1000)
            {
                await _laserLogic.SendData(new List<LaserMessage>() {new LaserMessage
                {
                    RedLaser = 255,
                    BlueLaser = 0,
                    GreenLaser = 100,
                    X = Convert.ToInt32(stopwatch.ElapsedMilliseconds),
                    Y = Convert.ToInt32(stopwatch.ElapsedMilliseconds)
                }});

                iterations++;
            }

            stopwatch.Stop();
            Assert.IsTrue(iterations > 25000); // 25000 is for 
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
                Assert.IsTrue(totalPower <= i && totalPower >= i - 3);
            }
        }

        [TestMethod]
        public void LimitTotalLaserPowerNecessaryTest2()
        {
            LaserMessage message = new()
            {
                RedLaser = 8,
                GreenLaser = 0,
                BlueLaser = 0
            };

            LaserLogic.LimitTotalLaserPowerIfNecessary(ref message, 6);
        }
    }
}
