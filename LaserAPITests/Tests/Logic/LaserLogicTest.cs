using LaserAPI.Logic;
using LaserAPI.Models.Helper.Laser;
using LaserAPITests.Mock;
using NUnit.Framework;

namespace LaserAPITests.Tests.Logic
{
    [TestFixture]
    internal class LaserLogicTest
    {
        private readonly LaserLogic _laserLogic;

        public LaserLogicTest()
        {
            _laserLogic = new MockLaserLogic().LaserLogic;
        }

        [Test]
        public void SendDataTest()
        {
            _laserLogic.SendData(new LaserMessage
            {
                RedLaser = 255,
                BlueLaser = 0,
                GreenLaser = 100,
                X = 0,
                Y = 2000
            });
        }
    }
}
