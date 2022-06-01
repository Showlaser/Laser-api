using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Helper;
using LaserAPI.Models.Helper.LaserAnimations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class PreMadeAnimationTest
    {
        [DataTestMethod]
        [DataRow(0, -4200, -4000)]
        [DataRow(0, 4200, 4000)]
        [DataRow(100, 4200, 4000)]
        [DataRow(-100, -4200, -4000)]
        [TestMethod]
        public void FixBoundaryNegativeTest(int center, int testValue, int expectedValue)
        {
            int value = PreMadeAnimation.FixBoundary(center, testValue);
            Assert.IsTrue(value == expectedValue);
        }

        [TestMethod]
        public void AnimationsAreGeneratedCorrect()
        {
            List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(e => e.GetTypes())
                .Where(e => typeof(IPreMadeLaserAnimation).IsAssignableFrom(e) && e.IsClass)
                .ToList();
            List<IPreMadeLaserAnimation> animations = new();
            animations.AddRange(types.Select(t => (IPreMadeLaserAnimation)Activator.CreateInstance(t)));

            PreMadeAnimationOptions options = new()
            {
                CenterX = 0,
                CenterY = 0,
                Rotation = 0,
                Scale = 0.5,
                SingleColor = false,
            };

            foreach (IPreMadeLaserAnimation? animation in animations)
            {
                AnimationDto preMadeAnimation = animation.GetAnimation(options);
                Assert.IsTrue(preMadeAnimation.PatternAnimations.Any());

                preMadeAnimation.PatternAnimations.ForEach(pa =>
                {
                    Assert.IsTrue();
                    pa.AnimationSettings.ForEach(ast => { Assert.IsTrue(ast.PatternAnimationUuid == pa.Uuid); });
                });
            }
        }
    }
}
