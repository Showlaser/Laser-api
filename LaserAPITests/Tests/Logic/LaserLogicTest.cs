using LaserAPI.Logic;
using LaserAPI.Models.Helper.Laser;
using LaserAPITests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    internal class LaserLogicTest
    {
        private readonly LaserLogic _laserLogic;

        public LaserLogicTest()
        {
            _laserLogic = new MockLaserLogic().LaserLogic;
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
    }
}
