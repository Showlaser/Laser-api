using LaserAPI.Models.Helper.Laser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LaserAPITests.Tests.Helper
{
    [TestClass]
    public class LaserSafetyHelperTest
    {
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

                LaserSafetyHelper.LimitLaserPowerPerLaserIfNecessary(ref message, i);
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

                LaserSafetyHelper.LimitTotalLaserPowerIfNecessary(ref message, i);
                int totalPower = message.RedLaser + message.GreenLaser + message.BlueLaser;
                Assert.IsTrue(totalPower <= i);
            }
        }
    }
}
