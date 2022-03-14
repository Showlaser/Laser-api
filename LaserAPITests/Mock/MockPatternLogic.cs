using LaserAPI.Interfaces.Dal;
using LaserAPI.Logic;
using LaserAPITests.MockedModels.Pattern;
using Moq;

namespace LaserAPITests.Mock
{
    internal class MockPatternLogic
    {
        internal PatternLogic PatternLogic;

        public MockPatternLogic()
        {
            MockedPattern? mockedPattern = new MockedPattern();
            Mock<IPatternDal>? patternDalMock = new Mock<IPatternDal>();
            patternDalMock.Setup(p => p.All()).ReturnsAsync(mockedPattern.PatternList);

            PatternLogic = new PatternLogic(patternDalMock.Object);
        }
    }
}