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

        /// <summary>
        /// Returns a fully valid pattern. Individual tests mutate a single property to
        /// assert that exactly that boundary is what makes the pattern invalid.
        /// </summary>
        private static PatternDto ValidPattern()
        {
            Guid patternUuid = Guid.NewGuid();
            return new PatternDto
            {
                Uuid = patternUuid,
                Name = "Pattern",
                Image = "image",
                Scale = 1.0,
                XOffset = 0,
                YOffset = 0,
                Rotation = 0,
                Points =
                [
                    new()
                    {
                        Uuid = Guid.NewGuid(),
                        PatternUuid = patternUuid,
                        X = 0,
                        Y = 0,
                        RedLaserPowerPwm = 0,
                        GreenLaserPowerPwm = 0,
                        BlueLaserPowerPwm = 0,
                        OrderNr = 0
                    }
                ]
            };
        }

        [TestMethod]
        public async Task AddTest()
        {
            await _patternLogic.AddOrUpdate(_pattern.Pattern);
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
        public async Task RemoveWithEmptyGuidThrowsTest()
        {
            await Assert.ThrowsAsync<NoNullAllowedException>(async () => await _patternLogic.Remove(Guid.Empty));
        }

        [TestMethod]
        public async Task AddOrUpdateWithInvalidPatternThrowsTest()
        {
            await Assert.ThrowsAsync<InvalidDataException>(async () => await _patternLogic.AddOrUpdate(_pattern.Empty));
        }

        [TestMethod]
        public async Task UpdateWithInvalidPatternThrowsTest()
        {
            await Assert.ThrowsAsync<InvalidDataException>(async () => await _patternLogic.Update(_pattern.Empty));
        }

        [TestMethod]
        public void PatternIsValidTest()
        {
            Assert.IsTrue(PatternLogic.PatternIsValid(ValidPattern()));
        }

        [TestMethod]
        public void PatternIsValidScaleBoundsAreInclusiveTest()
        {
            PatternDto low = ValidPattern();
            low.Scale = 0.1;
            PatternDto high = ValidPattern();
            high.Scale = 10;

            Assert.IsTrue(PatternLogic.PatternIsValid(low));
            Assert.IsTrue(PatternLogic.PatternIsValid(high));
        }

        [TestMethod]
        public void PatternIsInvalidWhenScaleTooHighTest()
        {
            PatternDto pattern = ValidPattern();
            pattern.Scale = 10.1;
            Assert.IsFalse(PatternLogic.PatternIsValid(pattern));
        }

        [TestMethod]
        public void PatternIsInvalidWhenScaleTooLowTest()
        {
            PatternDto pattern = ValidPattern();
            pattern.Scale = 0.05;
            Assert.IsFalse(PatternLogic.PatternIsValid(pattern));
        }

        [TestMethod]
        public void PatternIsInvalidWhenUuidIsEmptyTest()
        {
            PatternDto pattern = ValidPattern();
            pattern.Uuid = Guid.Empty;
            Assert.IsFalse(PatternLogic.PatternIsValid(pattern));
        }

        [TestMethod]
        public void PatternIsInvalidWhenNameIsEmptyTest()
        {
            PatternDto pattern = ValidPattern();
            pattern.Name = "";
            Assert.IsFalse(PatternLogic.PatternIsValid(pattern));
        }

        [TestMethod]
        public void PatternIsInvalidWhenImageIsEmptyTest()
        {
            PatternDto pattern = ValidPattern();
            pattern.Image = "";
            Assert.IsFalse(PatternLogic.PatternIsValid(pattern));
        }

        [TestMethod]
        public void PatternIsInvalidWhenRotationOutOfRangeTest()
        {
            PatternDto pattern = ValidPattern();
            pattern.Rotation = 361;
            Assert.IsFalse(PatternLogic.PatternIsValid(pattern));
        }

        [TestMethod]
        public void PatternIsInvalidWhenXOffsetOutOfRangeTest()
        {
            PatternDto pattern = ValidPattern();
            pattern.XOffset = 4001;
            Assert.IsFalse(PatternLogic.PatternIsValid(pattern));
        }

        [TestMethod]
        public void PatternIsInvalidWhenPointPwmOutOfRangeTest()
        {
            PatternDto pattern = ValidPattern();
            pattern.Points[0].RedLaserPowerPwm = 256;
            Assert.IsFalse(PatternLogic.PatternIsValid(pattern));
        }

        [TestMethod]
        public void PatternIsInvalidWhenPointCoordinateOutOfRangeTest()
        {
            PatternDto pattern = ValidPattern();
            pattern.Points[0].X = 4001;
            Assert.IsFalse(PatternLogic.PatternIsValid(pattern));
        }
    }
}
