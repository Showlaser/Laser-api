using LaserAPI.Logic;
using LaserAPI.Models.Dto.Lasershows;
using LaserAPITests.Mock;
using LaserAPITests.MockedModels.Lasershow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class LasershowLogicTest
    {
        private readonly LasershowLogic _lasershowLogic = new MockedLasershowLogic().LasershowLogic;

        [TestMethod]
        public async Task AddOrUpdateWithValidLasershowSucceedsTest()
        {
            await _lasershowLogic.AddOrUpdate(new MockedLasershow().Lasershow);
        }

        [TestMethod]
        public async Task AddOrUpdateWithInvalidLasershowThrowsTest()
        {
            LasershowDto lasershow = new MockedLasershow().Lasershow;
            lasershow.Name = "";

            await Assert.ThrowsAsync<InvalidDataException>(
                async () => await _lasershowLogic.AddOrUpdate(lasershow));
        }

        [TestMethod]
        public async Task AddOrUpdateWithInvalidTimelineIdThrowsTest()
        {
            LasershowDto lasershow = new MockedLasershow().Lasershow;
            lasershow.LasershowAnimations[0].TimelineId = 4;

            await Assert.ThrowsAsync<InvalidDataException>(
                async () => await _lasershowLogic.AddOrUpdate(lasershow));
        }

        [TestMethod]
        public async Task AddOrUpdateWithEmptyLasershowThrowsTest()
        {
            // An empty show has a null LasershowAnimations list; this must surface as a
            // validation failure, not a NullReferenceException.
            await Assert.ThrowsAsync<InvalidDataException>(
                async () => await _lasershowLogic.AddOrUpdate(new MockedLasershow().Empty));
        }

        [TestMethod]
        public async Task AllTest()
        {
            var lasershows = await _lasershowLogic.All();
            Assert.IsTrue(lasershows.Count > 0);
        }
    }
}
