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
            MockedPattern mockedPattern = new();
            Mock<IPatternDal> patternDalMock = new();
            patternDalMock.Setup(p => p.All()).ReturnsAsync(mockedPattern.PatternList);

            PatternLogic = new PatternLogic(patternDalMock.Object, null);
        }
    }
}