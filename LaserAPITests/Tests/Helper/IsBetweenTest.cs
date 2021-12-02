using LaserAPI.Models.Helper;
using LaserAPITests.MockedModels.Pattern;
using NUnit.Framework;
using System.Data;

namespace LaserAPITests.Tests.Helper
{
    [TestFixture]
    internal class IsBetweenTest
    {
        private readonly MockedPattern _pattern = new();

        [Test]
        public void IsBetweenOrEqualTo()
        {
            Assert.True(_pattern.Pattern.Scale.IsBetweenOrEqualTo(0.1, 1));
        }

        [Test]
        public void IsBetweenOrEqualToNull()
        {
            Assert.Throws<NoNullAllowedException>(() => _pattern.Empty.Scale.IsBetweenOrEqualTo(0.1, 1));
        }

        [Test]
        public void IsBetweenOrEqualToToHigh()
        {
            Assert.IsFalse(_pattern.ScaleToHigh.Scale.IsBetweenOrEqualTo(0.1, 1));
        }

        [Test]
        public void IsBetweenOrEqualToToLow()
        {
            Assert.IsFalse(_pattern.ScaleToLow.Scale.IsBetweenOrEqualTo(0.1, 1));
        }
    }
}
