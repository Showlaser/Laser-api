using LaserAPI.Interfaces;
using LaserAPITests.MockedModels.Animation;
using Moq;

namespace LaserAPITests.Mock
{
    internal class MockedLaserConnectionLogic
    {
        public readonly ILaserConnectionLogic LaserConnectionLogic;

        public MockedLaserConnectionLogic()
        {
            MockedAnimation mockedAnimation = new();
            Mock<ILaserConnectionLogic> mockedLaserConnectionLogic = new();
            LaserConnectionLogic = mockedLaserConnectionLogic.Object;
        }
    }
}
