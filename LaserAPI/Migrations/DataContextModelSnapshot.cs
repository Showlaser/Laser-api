﻿// <auto-generated />
using System;
using LaserAPI.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LaserAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("LaserAPI.Models.Dto.Animations.AnimationDto", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Uuid");

                    b.ToTable("Animation");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Animations.AnimationPointDto", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("BlueLaserPowerPwm")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GreenLaserPowerPwm")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("PatternAnimationSettingsUuid")
                        .HasColumnType("TEXT");

                    b.Property<int>("RedLaserPowerPwm")
                        .HasColumnType("INTEGER");

                    b.Property<int>("X")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Y")
                        .HasColumnType("INTEGER");

                    b.HasKey("Uuid");

                    b.HasIndex("PatternAnimationSettingsUuid");

                    b.ToTable("AnimationPointDto");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Animations.PatternAnimationDto", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AnimationUuid")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("StartTimeOffset")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimeLineId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Uuid");

                    b.HasIndex("AnimationUuid");

                    b.ToTable("PatternAnimationDto");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Animations.PatternAnimationSettingsDto", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("CenterX")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CenterY")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("PatternAnimationDtoUuid")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PatternAnimationUuid")
                        .HasColumnType("TEXT");

                    b.Property<double>("Scale")
                        .HasColumnType("REAL");

                    b.Property<int>("StartTime")
                        .HasColumnType("INTEGER");

                    b.HasKey("Uuid");

                    b.HasIndex("PatternAnimationDtoUuid");

                    b.ToTable("PatternAnimationSettingsDto");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Patterns.PatternDto", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double>("Scale")
                        .HasColumnType("REAL");

                    b.HasKey("Uuid");

                    b.ToTable("Pattern");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Patterns.PointDto", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("BlueLaserPowerPwm")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GreenLaserPowerPwm")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("PatternUuid")
                        .HasColumnType("TEXT");

                    b.Property<int>("RedLaserPowerPwm")
                        .HasColumnType("INTEGER");

                    b.Property<int>("X")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Y")
                        .HasColumnType("INTEGER");

                    b.HasKey("Uuid");

                    b.HasIndex("PatternUuid");

                    b.ToTable("PointDto");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Zones.ZoneDto", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxLaserPowerInZonePwm")
                        .HasColumnType("INTEGER");

                    b.HasKey("Uuid");

                    b.ToTable("Zone");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Zones.ZonesPositionDto", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("X")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Y")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ZoneUuid")
                        .HasColumnType("TEXT");

                    b.HasKey("Uuid");

                    b.HasIndex("ZoneUuid");

                    b.ToTable("ZonesPositionDto");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Animations.AnimationPointDto", b =>
                {
                    b.HasOne("LaserAPI.Models.Dto.Animations.PatternAnimationSettingsDto", null)
                        .WithMany("Points")
                        .HasForeignKey("PatternAnimationSettingsUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Animations.PatternAnimationDto", b =>
                {
                    b.HasOne("LaserAPI.Models.Dto.Animations.AnimationDto", null)
                        .WithMany("PatternAnimations")
                        .HasForeignKey("AnimationUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Animations.PatternAnimationSettingsDto", b =>
                {
                    b.HasOne("LaserAPI.Models.Dto.Animations.PatternAnimationDto", null)
                        .WithMany("AnimationSettings")
                        .HasForeignKey("PatternAnimationDtoUuid");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Patterns.PointDto", b =>
                {
                    b.HasOne("LaserAPI.Models.Dto.Patterns.PatternDto", null)
                        .WithMany("Points")
                        .HasForeignKey("PatternUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Zones.ZonesPositionDto", b =>
                {
                    b.HasOne("LaserAPI.Models.Dto.Zones.ZoneDto", null)
                        .WithMany("Positions")
                        .HasForeignKey("ZoneUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Animations.AnimationDto", b =>
                {
                    b.Navigation("PatternAnimations");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Animations.PatternAnimationDto", b =>
                {
                    b.Navigation("AnimationSettings");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Animations.PatternAnimationSettingsDto", b =>
                {
                    b.Navigation("Points");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Patterns.PatternDto", b =>
                {
                    b.Navigation("Points");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Zones.ZoneDto", b =>
                {
                    b.Navigation("Positions");
                });
#pragma warning restore 612, 618
        }
    }
}
