using LaserAPI.Interfaces.Dal;
using LaserAPI.Logic;
using LaserAPITests.MockedModels.Lasershow;
using Moq;

namespace LaserAPITests.Mock
{
    internal class MockedLasershowLogic
    {
        internal LasershowLogic LasershowLogic;

        public MockedLasershowLogic()
        {
            MockedLasershow mockedLasershow = new();
            Mock<ILasershowDal> lasershowDalMock = new();
            lasershowDalMock.Setup(d => d.All()).ReturnsAsync(mockedLasershow.LasershowList);

            LasershowLogic = new LasershowLogic(lasershowDalMock.Object);
        }
    }
}
