﻿// <auto-generated />
using System;
using LaserAPI.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LaserAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20211226150553_patternName")]
    partial class patternName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

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

                    b.Property<Guid>("ConnectedToUuid")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PatternUuid")
                        .HasColumnType("TEXT");

                    b.Property<short>("X")
                        .HasColumnType("INTEGER");

                    b.Property<short>("Y")
                        .HasColumnType("INTEGER");

                    b.HasKey("Uuid");

                    b.HasIndex("PatternUuid");

                    b.ToTable("PointDto");
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Patterns.PointDto", b =>
                {
                    b.HasOne("LaserAPI.Models.Dto.Patterns.PatternDto", null)
                        .WithMany("Points")
                        .HasForeignKey("PatternUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LaserAPI.Models.Dto.Patterns.PatternDto", b =>
                {
                    b.Navigation("Points");
                });
#pragma warning restore 612, 618
        }
    }
}
