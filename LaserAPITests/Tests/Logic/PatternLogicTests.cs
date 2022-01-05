using LaserAPI.Logic;
using LaserAPI.Models.Dto.Patterns;
using LaserAPITests.Mock;
using LaserAPITests.MockedModels.Pattern;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPITests.Logic
{
    [TestFixture]
    public class PatternLogicTests
    {
        private readonly PatternLogic _patternLogic = new MockPatternLogic().PatternLogic;
        private readonly MockedPattern _pattern = new();
        private readonly List<PatternDto> _patternList = new MockedPattern().PatternList;

        [Test]
        public void AddTest()
        {
            Assert.DoesNotThrowAsync(async () => await _patternLogic.AddOrUpdate(_pattern.Pattern));
        }

        [Test]
        public void AddTestEmpty()
        {
            Assert.ThrowsAsync<NoNullAllowedException>(async () => await _patternLogic.AddOrUpdate(_pattern.Empty));
        }

        [Test]
        public void AddTestScaleToHigh()
        {
            Assert.ThrowsAsync<InvalidDataException>(async () => await _patternLogic.AddOrUpdate(_pattern.ScaleToHigh));
        }

        [Test]
        public void AddTestScaleToLow()
        {
            Assert.ThrowsAsync<InvalidDataException>(async () => await _patternLogic.AddOrUpdate(_pattern.ScaleToLow));
        }

        [Test]
        public void AddTestEmptyPoints()
        {
            Assert.ThrowsAsync<InvalidDataException>(async () => await _patternLogic.AddOrUpdate(_pattern.EmptyPoints));
        }

        [Test]
        public async Task AllTest()
        {
            List<PatternDto> patterns = await _patternLogic.All();
            Assert.True(patterns.Any());
        }

        [Test]
        public void UpdateTest()
        {
            Assert.DoesNotThrowAsync(async () => await _patternLogic.Update(_pattern.Pattern));
        }

        [Test]
        public void UpdateTestEmpty()
        {
            Assert.ThrowsAsync<NoNullAllowedException>(async () => await _patternLogic.Update(_pattern.Empty));
        }

        [Test]
        public void UpdateTestScaleToHigh()
        {
            Assert.ThrowsAsync<InvalidDataException>(async () => await _patternLogic.Update(_pattern.ScaleToHigh));
        }

        [Test]
        public void UpdateTestScaleToLow()
        {
            Assert.ThrowsAsync<InvalidDataException>(async () => await _patternLogic.Update(_pattern.ScaleToLow));
        }

        [Test]
        public void UpdateTestEmptyPoints()
        {
            Assert.ThrowsAsync<InvalidDataException>(async () => await _patternLogic.Update(_pattern.EmptyPoints));
        }

        [Test]
        public void RemoveTest()
        {
            Assert.ThrowsAsync<NoNullAllowedException>(async () => await _patternLogic.Remove(Guid.Empty));
        }
    }
}