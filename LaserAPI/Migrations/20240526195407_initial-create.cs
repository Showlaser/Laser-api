﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace LaserAPI.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animation",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animation", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Lasershow",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lasershow", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "LasershowSpotifyConnector",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    LasershowUuid = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LasershowSpotifyConnector", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Pattern",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<string>(type: "TEXT", nullable: true),
                    Scale = table.Column<double>(type: "REAL", nullable: false),
                    XOffset = table.Column<int>(type: "INTEGER", nullable: false),
                    YOffset = table.Column<int>(type: "INTEGER", nullable: false),
                    Rotation = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pattern", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Zone",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    AppliedOnShowLaserUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    MaxLaserPowerInZonePercentage = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zone", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "PatternAnimation",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnimationUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    PatternUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    StartTimeMs = table.Column<int>(type: "INTEGER", nullable: false),
                    TimelineId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatternAnimation", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_PatternAnimation_Animation_AnimationUuid",
                        column: x => x.AnimationUuid,
                        principalTable: "Animation",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LasershowAnimation",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    LasershowUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnimationUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    StartTimeMs = table.Column<int>(type: "INTEGER", nullable: false),
                    TimelineId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LasershowAnimation", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_LasershowAnimation_Lasershow_LasershowUuid",
                        column: x => x.LasershowUuid,
                        principalTable: "Lasershow",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LasershowSpotifyConnectorSongDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    LasershowSpotifyConnectorUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    SpotifySongId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LasershowSpotifyConnectorSongDto", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_LasershowSpotifyConnectorSongDto_LasershowSpotifyConnector_LasershowSpotifyConnectorUuid",
                        column: x => x.LasershowSpotifyConnectorUuid,
                        principalTable: "LasershowSpotifyConnector",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Point",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    PatternUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    RedLaserPowerPwm = table.Column<int>(type: "INTEGER", nullable: false),
                    GreenLaserPowerPwm = table.Column<int>(type: "INTEGER", nullable: false),
                    BlueLaserPowerPwm = table.Column<int>(type: "INTEGER", nullable: false),
                    ConnectedToPointUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderNr = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Point", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Point_Pattern_PatternUuid",
                        column: x => x.PatternUuid,
                        principalTable: "Pattern",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZonePosition",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    SafetyZoneUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderNr = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZonePosition", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_ZonePosition_Zone_SafetyZoneUuid",
                        column: x => x.SafetyZoneUuid,
                        principalTable: "Zone",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimationPatternKeyFrameDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnimationPatternUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimeMs = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyEdited = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyValue = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimationPatternKeyFrameDto", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_AnimationPatternKeyFrameDto_PatternAnimation_AnimationPatternUuid",
                        column: x => x.AnimationPatternUuid,
                        principalTable: "PatternAnimation",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimationPatternKeyFrameDto_AnimationPatternUuid",
                table: "AnimationPatternKeyFrameDto",
                column: "AnimationPatternUuid");

            migrationBuilder.CreateIndex(
                name: "IX_LasershowAnimation_LasershowUuid",
                table: "LasershowAnimation",
                column: "LasershowUuid");

            migrationBuilder.CreateIndex(
                name: "IX_LasershowSpotifyConnectorSongDto_LasershowSpotifyConnectorUuid",
                table: "LasershowSpotifyConnectorSongDto",
                column: "LasershowSpotifyConnectorUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PatternAnimation_AnimationUuid",
                table: "PatternAnimation",
                column: "AnimationUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Point_PatternUuid",
                table: "Point",
                column: "PatternUuid");

            migrationBuilder.CreateIndex(
                name: "IX_ZonePosition_SafetyZoneUuid",
                table: "ZonePosition",
                column: "SafetyZoneUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimationPatternKeyFrameDto");

            migrationBuilder.DropTable(
                name: "LasershowAnimation");

            migrationBuilder.DropTable(
                name: "LasershowSpotifyConnectorSongDto");

            migrationBuilder.DropTable(
                name: "Point");

            migrationBuilder.DropTable(
                name: "ZonePosition");

            migrationBuilder.DropTable(
                name: "PatternAnimation");

            migrationBuilder.DropTable(
                name: "Lasershow");

            migrationBuilder.DropTable(
                name: "LasershowSpotifyConnector");

            migrationBuilder.DropTable(
                name: "Pattern");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropTable(
                name: "Animation");
        }
    }
}
