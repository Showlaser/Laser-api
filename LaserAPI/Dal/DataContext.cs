using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.LasershowSpotify;
using LaserAPI.Models.Dto.Patterns;
using LaserAPI.Models.Dto.Zones;
using Microsoft.EntityFrameworkCore;

namespace LaserAPI.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<AnimationDto> Animation { get; set; }
        public virtual DbSet<AnimationPatternDto> PatternAnimation { get; set; }
        public virtual DbSet<PatternDto> Pattern { get; set; }
        public virtual DbSet<PointDto> Point { get; set; }
        public virtual DbSet<ZoneDto> Zone { get; set; }
        public virtual DbSet<ZonesPositionDto> ZonePosition { get; set; }
        public virtual DbSet<LasershowSpotifyConnectorDto> LasershowSpotifyConnector { get; set; }

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
                e.HasMany(a => a.AnimationPatterns)
                    .WithOne()
                    .HasForeignKey(a => a.AnimationUuid);
            });

            builder.Entity<AnimationPatternDto>(e =>
            {
                e.HasKey(ap => ap.Uuid);
                e.HasMany(ap => ap.AnimationPatternKeyFrames)
                .WithOne()
                .HasForeignKey(ap => ap.AnimationPatternUuid);
            });

            builder.Entity<AnimationPatternDto>(e =>
            {
                e.HasKey(ap => ap.Uuid);
                e.HasMany(ap => ap.AnimationPatternKeyFrames)
                .WithOne()
                .HasForeignKey(ap => ap.AnimationPatternUuid);
            });

            builder.Entity<AnimationPatternKeyFrameDto>(e =>
            {
                e.HasKey(apkf => apkf.Uuid);
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

            builder.Entity<LasershowSpotifyConnectorDto>(e =>
            {
                e.HasKey(lsc => lsc.Uuid);
                e.HasMany(lsc => lsc.SpotifySongs)
                    .WithOne()
                    .HasForeignKey(lscs => lscs.LasershowSpotifyConnectorUuid);
            });

            builder.Entity<LasershowSpotifyConnectorSongDto>(e =>
            {
                e.HasKey(lscs => lscs.Uuid);
            });
        }
    }
}