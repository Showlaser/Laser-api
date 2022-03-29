using LaserAPI.Models.Helper.Laser;
using LaserAPITests.MockedModels.LaserMessage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LaserAPITests.Tests.Helper
{
    [TestClass]
    public class LaserSafetyHelperTest
    {
        private readonly MockedLaserMessage _laserMessage = new();

        [TestMethod]
        public void LimitLaserPowerIfNecessaryTest()
        {
            LaserMessage message = _laserMessage.LaserMessage;
            for (int i = 0; i < 255; i++)
            {
                LaserSafetyHelper.LimitLaserPowerIfNecessary(ref message, i);
                Assert.IsTrue(message.RedLaser <= i);
                Assert.IsTrue(message.GreenLaser <= i);
                Assert.IsTrue(message.BlueLaser <= i);
            }
        }
    }
}
