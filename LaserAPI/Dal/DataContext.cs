using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Lasershow;
using LaserAPI.Models.Dto.Patterns;
using LaserAPI.Models.Dto.Zones;
using Microsoft.EntityFrameworkCore;

namespace LaserAPI.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<AnimationDto> Animation { get; set; }
        public virtual DbSet<PatternAnimationDto> PatternAnimation { get; set; }
        public virtual DbSet<PatternAnimationSettingsDto> PatternAnimationSetting { get; set; }
        public virtual DbSet<AnimationPointDto> AnimationPoint { get; set; }
        public virtual DbSet<PatternDto> Pattern { get; set; }
        public virtual DbSet<ZoneDto> Zone { get; set; }
        public virtual DbSet<ZonesPositionDto> ZonePosition { get; set; }
        public virtual DbSet<LasershowDto> Lasershow { get; set; }
        public virtual DbSet<LasershowAnimationDto> LasershowAnimation { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LasershowDto>(e =>
            {
                e.HasKey(l => l.Uuid);
                e.HasMany(l => l.Animations)
                    .WithOne()
                    .HasForeignKey(a => a.LasershowUuid);
            });

            builder.Entity<LasershowAnimationDto>(e =>
            {
                e.HasKey(l => l.Uuid);
            });

            builder.Entity<LasershowAnimationDto>(e =>
            {
                e.HasKey(p => p.Uuid);
            });

            builder.Entity<PatternDto>(e =>
            {
                e.HasKey(p => p.Uuid);
            });

            builder.Entity<PointDto>(e =>
            {
                e.HasKey(p => p.Uuid);
            });

            builder.Entity<AnimationDto>(e =>
            {
                e.HasKey(a => a.Uuid);
                e.HasMany(a => a.PatternAnimations)
                    .WithOne()
                    .HasForeignKey(a => a.AnimationUuid);
            });

            builder.Entity<PatternAnimationDto>(e =>
            {
                e.HasKey(pa => pa.Uuid);
                e.HasMany(a => a.AnimationSettings)
                    .WithOne()
                    .HasForeignKey(a => a.PatternAnimationUuid);
            });

            builder.Entity<PatternAnimationSettingsDto>(e =>
            {
                e.HasKey(pas => pas.Uuid);
                e.HasMany(a => a.Points)
                    .WithOne()
                    .HasForeignKey(a => a.PatternAnimationSettingsUuid);
            });

            builder.Entity<AnimationPointDto>(e =>
            {
                e.HasKey(p => p.Uuid);
            });

            builder.Entity<ZoneDto>(e =>
            {
                e.HasKey(z => z.Uuid);
                e.HasMany(z => z.Points)
                    .WithOne()
                    .HasForeignKey(p => p.ZoneUuid);
            });

            builder.Entity<ZonesPositionDto>(e =>
            {
                e.HasKey(zp => zp.Uuid);
            });
        }
    }
}