using LaserAPI.Dal;
using LaserAPI.Logic;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Lasershows;
using LaserAPI.Models.Dto.Patterns;
using LaserAPITests.Mock;
using LaserAPITests.MockedModels.Animation;
using LaserAPITests.MockedModels.Lasershow;
using LaserAPITests.MockedModels.Pattern;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPITests.Tests.Component
{
    /// <summary>
    /// Component tests that drive the same path the controllers use (Logic.AddOrUpdate for the
    /// POST endpoints, Logic.All for the GET endpoints) against a real EF Core context on an
    /// in-memory SQLite database. Where the Moq-based logic tests only confirm validation, these
    /// confirm that a lasershow, its animations and their patterns are actually persisted and
    /// reloaded with their relationships intact.
    /// </summary>
    [TestClass]
    public class LoadFromDatabaseComponentTests
    {
        private MockedDataContext _dataContext = null!;

        // The mocked test data uses fixed UUIDs that tie the graph together; capture them so the
        // assertions can locate the exact rows that were seeded.
        private static readonly Guid PatternUuid = new MockedPattern().Pattern.Uuid;
        private static readonly Guid AnimationUuid = new MockedAnimation().Animation.Uuid;
        private static readonly Guid LasershowUuid = new MockedLasershow().Lasershow.Uuid;

        [TestInitialize]
        public void Setup()
        {
            _dataContext = new MockedDataContext();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dataContext.Dispose();
        }

        private PatternLogic NewPatternLogic(DataContext context) =>
            new(new PatternDal(context), new AnimationDal(context));

        private AnimationLogic NewAnimationLogic(DataContext context) =>
            new(new AnimationDal(context));

        private LasershowLogic NewLasershowLogic(DataContext context) =>
            new(new LasershowDal(context));

        /// <summary>
        /// Seeds the shared library bottom-up: the pattern first, then an animation referencing
        /// that pattern, then a lasershow referencing that animation. Each step runs on its own
        /// context, exactly as a scoped DAL would per request.
        /// </summary>
        private async Task SeedFullGraphAsync()
        {
            await using (DataContext context = _dataContext.NewContext())
            {
                await NewPatternLogic(context).AddOrUpdate(new MockedPattern().Pattern);
            }

            await using (DataContext context = _dataContext.NewContext())
            {
                await NewAnimationLogic(context).AddOrUpdate(new MockedAnimation().Animation);
            }

            await using (DataContext context = _dataContext.NewContext())
            {
                await NewLasershowLogic(context).AddOrUpdate(new MockedLasershow().Lasershow);
            }
        }

        [TestMethod]
        public async Task PatternIsLoadedWithItsPointsFromDatabaseTest()
        {
            await using (DataContext context = _dataContext.NewContext())
            {
                await NewPatternLogic(context).AddOrUpdate(new MockedPattern().Pattern);
            }

            await using DataContext readContext = _dataContext.NewContext();
            List<PatternDto> patterns = await NewPatternLogic(readContext).All();

            PatternDto pattern = patterns.Single(p => p.Uuid == PatternUuid);
            Assert.IsNotNull(pattern.Points, "Points navigation should be loaded.");
            Assert.IsTrue(pattern.Points.Count > 0, "The seeded pattern should reload with its points.");
            Assert.IsTrue(pattern.Points.TrueForAll(point => point.PatternUuid == PatternUuid),
                "Every loaded point should be linked back to its pattern.");
        }

        [TestMethod]
        public async Task AnimationIsLoadedWithItsPatternAndKeyFramesFromDatabaseTest()
        {
            await using (DataContext context = _dataContext.NewContext())
            {
                await NewPatternLogic(context).AddOrUpdate(new MockedPattern().Pattern);
            }

            await using (DataContext context = _dataContext.NewContext())
            {
                await NewAnimationLogic(context).AddOrUpdate(new MockedAnimation().Animation);
            }

            await using DataContext readContext = _dataContext.NewContext();
            List<AnimationDto> animations = await NewAnimationLogic(readContext).All();

            AnimationDto animation = animations.Single(a => a.Uuid == AnimationUuid);
            Assert.IsNotNull(animation.AnimationPatterns, "AnimationPatterns navigation should be loaded.");
            Assert.IsTrue(animation.AnimationPatterns.Count > 0);

            AnimationPatternDto animationPattern = animation.AnimationPatterns.First();
            Assert.IsTrue(animationPattern.AnimationPatternKeyFrames.Count > 0,
                "The animation pattern should reload with its key frames.");

            // The animation references a shared pattern by foreign key; loading must rehydrate it.
            Assert.IsNotNull(animationPattern.Pattern, "The referenced pattern should be loaded.");
            Assert.AreEqual(PatternUuid, animationPattern.Pattern.Uuid);
            Assert.IsTrue(animationPattern.Pattern.Points.Count > 0,
                "The pattern reached through the animation should carry its points.");
        }

        [TestMethod]
        public async Task LasershowIsLoadedWithAnimationAndPatternFromDatabaseTest()
        {
            await SeedFullGraphAsync();

            await using DataContext readContext = _dataContext.NewContext();
            List<LasershowDto> lasershows = await NewLasershowLogic(readContext).All();

            LasershowDto lasershow = lasershows.Single(l => l.Uuid == LasershowUuid);
            Assert.IsNotNull(lasershow.LasershowAnimations, "LasershowAnimations navigation should be loaded.");
            Assert.IsTrue(lasershow.LasershowAnimations.Count > 0);

            LasershowAnimationDto lasershowAnimation = lasershow.LasershowAnimations.First();

            // Lasershow -> Animation: the animation is referenced by foreign key and must be rehydrated.
            Assert.IsNotNull(lasershowAnimation.Animation, "The lasershow's animation should be loaded.");
            Assert.AreEqual(AnimationUuid, lasershowAnimation.Animation.Uuid);

            // Animation -> AnimationPattern -> Pattern: the full chain must come back from the database.
            Assert.IsTrue(lasershowAnimation.Animation.AnimationPatterns.Count > 0,
                "The loaded animation should contain its animation patterns.");

            AnimationPatternDto animationPattern = lasershowAnimation.Animation.AnimationPatterns.First();
            Assert.IsNotNull(animationPattern.Pattern, "The animation's pattern should be loaded.");
            Assert.AreEqual(PatternUuid, animationPattern.Pattern.Uuid);
            Assert.IsTrue(animationPattern.Pattern.Points.Count > 0,
                "The pattern reached through the lasershow should carry its points.");
            Assert.IsTrue(animationPattern.AnimationPatternKeyFrames.Count > 0,
                "The animation pattern reached through the lasershow should carry its key frames.");
        }

        [TestMethod]
        public async Task LasershowAnimationsReferenceTheSameSharedAnimationTest()
        {
            await SeedFullGraphAsync();

            await using DataContext readContext = _dataContext.NewContext();
            List<LasershowDto> lasershows = await NewLasershowLogic(readContext).All();

            LasershowDto lasershow = lasershows.Single(l => l.Uuid == LasershowUuid);

            // The mocked lasershow places two animations on the timeline that point at the same
            // shared animation; both must resolve to the same animation row after loading.
            Assert.IsTrue(lasershow.LasershowAnimations.Count > 1,
                "The seeded lasershow should contain more than one timeline animation.");
            Assert.IsTrue(lasershow.LasershowAnimations.TrueForAll(la => la.Animation != null),
                "Every timeline animation should have its animation loaded.");
            Assert.IsTrue(lasershow.LasershowAnimations.TrueForAll(la => la.Animation.Uuid == AnimationUuid),
                "Every timeline animation should reference the same shared animation.");
        }
    }
}
