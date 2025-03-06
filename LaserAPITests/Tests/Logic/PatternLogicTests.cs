using LaserAPI.Logic;
using LaserAPI.Models.Dto.Patterns;
using LaserAPITests.Mock;
using LaserAPITests.MockedModels.Pattern;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class PatternLogicTests
    {
        private readonly PatternLogic _patternLogic = new MockedPatternLogic().PatternLogic;
        private readonly MockedPattern _pattern = new();

        [TestMethod]
        public async Task AddTest()
        {
            await _patternLogic.AddOrUpdate(_pattern.Pattern);
        }

        [TestMethod]
        public void AddTestEmpty()
        {
            Assert.ThrowsExceptionAsync<NoNullAllowedException>(async () => await _patternLogic.AddOrUpdate(_pattern.Empty));
        }

        [TestMethod]
        public void AddTestScaleToHigh()
        {
            Assert.ThrowsExceptionAsync<InvalidDataException>(async () => await _patternLogic.AddOrUpdate(_pattern.ScaleToHigh));
        }

        [TestMethod]
        public void AddTestScaleToLow()
        {
            Assert.ThrowsExceptionAsync<InvalidDataException>(async () => await _patternLogic.AddOrUpdate(_pattern.ScaleToLow));
        }

        [TestMethod]
        public void AddTestEmptyPoints()
        {
            Assert.ThrowsExceptionAsync<InvalidDataException>(async () => await _patternLogic.AddOrUpdate(_pattern.EmptyPoints));
        }

        [TestMethod]
        public async Task AllTest()
        {
            List<PatternDto> patterns = await _patternLogic.All();
            Assert.IsTrue(patterns.Any());
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            await _patternLogic.Update(_pattern.Pattern);
        }

        [TestMethod]
        public void UpdateTestEmpty()
        {
            Assert.ThrowsExceptionAsync<NoNullAllowedException>(async () => await _patternLogic.Update(_pattern.Empty));
        }

        [TestMethod]
        public void UpdateTestScaleToHigh()
        {
            Assert.ThrowsExceptionAsync<InvalidDataException>(async () => await _patternLogic.Update(_pattern.ScaleToHigh));
        }

        [TestMethod]
        public void UpdateTestScaleToLow()
        {
            Assert.ThrowsExceptionAsync<InvalidDataException>(async () => await _patternLogic.Update(_pattern.ScaleToLow));
        }

        [TestMethod]
        public void UpdateTestEmptyPoints()
        {
            Assert.ThrowsExceptionAsync<InvalidDataException>(async () => await _patternLogic.Update(_pattern.EmptyPoints));
        }

        [TestMethod]
        public void RemoveTest()
        {
            Assert.ThrowsExceptionAsync<NoNullAllowedException>(async () => await _patternLogic.Remove(Guid.Empty));
        }
    }
}