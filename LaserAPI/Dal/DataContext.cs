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
        public virtual DbSet<PatternDto> Pattern { get; set; }
        public virtual DbSet<AnimationDto> Animation { get; set; }
        public virtual DbSet<ZoneDto> Zone { get; set; }
        public virtual DbSet<LasershowDto> Lasershow { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LasershowDto>(e =>
            {
                e.HasKey(p => p.Uuid);
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
            });

            builder.Entity<PatternAnimationDto>(e =>
            {
                e.HasKey(pa => pa.Uuid);
            });

            builder.Entity<PatternAnimationSettingsDto>(e =>
            {
                e.HasKey(pas => pas.Uuid);
            });

            builder.Entity<AnimationPointDto>(e =>
            {
                e.HasKey(p => p.Uuid);
            });

            builder.Entity<ZoneDto>(e =>
            {
                e.HasKey(z => z.Uuid);
            });

            builder.Entity<ZonesPositionDto>(e =>
            {
                e.HasKey(zp => zp.Uuid);
            });
        }
    }
}