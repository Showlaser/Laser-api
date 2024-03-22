using LaserAPI.Logic;
using LaserAPITests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class LaserLogicTest
    {
        private readonly LaserLogic _laserLogic = new MockedLaserLogic().LaserLogic;
    }
}
