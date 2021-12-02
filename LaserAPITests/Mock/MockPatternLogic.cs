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
            var mockedPattern = new MockedPattern();
            var patternDalMock = new Mock<IPatternDal>();
            patternDalMock.Setup(p => p.All()).ReturnsAsync(mockedPattern.PatternList);

            PatternLogic = new PatternLogic(patternDalMock.Object);
        }
    }
}