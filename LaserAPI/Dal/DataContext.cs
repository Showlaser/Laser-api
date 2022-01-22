using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Patterns;
using Microsoft.EntityFrameworkCore;

namespace LaserAPI.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<PatternDto> Pattern { get; set; }
        public virtual DbSet<AnimationDto> Animation { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PatternDto>(e =>
            {
                e.HasKey(p => p.Uuid);
                e.HasMany(p => p.Points)
                    .WithOne()
                    .HasForeignKey(p => p.PatternUuid);
            });

            builder.Entity<PointDto>(e =>
            {
                e.HasKey(p => p.Uuid);
            });

            builder.Entity<AnimationDto>(e =>
            {
                e.HasKey(a => a.Uuid);
                e.HasMany(a => a.AnimationTimeline)
                    .WithOne()
                    .HasForeignKey(a => a.AnimationUuid);
            });

            builder.Entity<AnimationTimelineDto>(e =>
            {
                e.HasKey(pa => pa.Uuid);
            });

            builder.Entity<TimelineSettingsDto>(e =>
            {
                e.HasKey(pas => pas.Uuid);
                e.HasMany(pas => pas.Points)
                    .WithOne()
                    .HasForeignKey(pas => pas.TimelineSettingsUuid);
            });

            builder.Entity<AnimationPointDto>(e =>
            {
                e.HasKey(p => p.Uuid);
            });
        }
    }
}