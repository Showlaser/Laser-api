﻿using LaserAPI.Logic;
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
    }
}
