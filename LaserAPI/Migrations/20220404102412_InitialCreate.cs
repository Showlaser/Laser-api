using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animation",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
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
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lasershow", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Pattern",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Scale = table.Column<double>(type: "REAL", nullable: false)
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
                    MaxLaserPowerInZonePwm = table.Column<int>(type: "INTEGER", nullable: false)
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
                    StartTimeOffset = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeLineId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    AnimationDtoUuid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatternAnimation", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_PatternAnimation_Animation_AnimationDtoUuid",
                        column: x => x.AnimationDtoUuid,
                        principalTable: "Animation",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "LasershowAnimationDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    StartTime = table.Column<int>(type: "INTEGER", nullable: false),
                    TimelineId = table.Column<int>(type: "INTEGER", nullable: false),
                    AnimationUuid = table.Column<Guid>(type: "TEXT", nullable: true),
                    LasershowDtoUuid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LasershowAnimationDto", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_LasershowAnimationDto_Animation_AnimationUuid",
                        column: x => x.AnimationUuid,
                        principalTable: "Animation",
                        principalColumn: "Uuid");
                    table.ForeignKey(
                        name: "FK_LasershowAnimationDto_Lasershow_LasershowDtoUuid",
                        column: x => x.LasershowDtoUuid,
                        principalTable: "Lasershow",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "PointDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    PatternUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    RedLaserPowerPwm = table.Column<int>(type: "INTEGER", nullable: false),
                    GreenLaserPowerPwm = table.Column<int>(type: "INTEGER", nullable: false),
                    BlueLaserPowerPwm = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    PatternDtoUuid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointDto", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_PointDto_Pattern_PatternDtoUuid",
                        column: x => x.PatternDtoUuid,
                        principalTable: "Pattern",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "ZonesPositionDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    ZoneUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    ZoneDtoUuid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZonesPositionDto", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_ZonesPositionDto_Zone_ZoneDtoUuid",
                        column: x => x.ZoneDtoUuid,
                        principalTable: "Zone",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "PatternAnimationSetting",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    PatternAnimationUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Scale = table.Column<double>(type: "REAL", nullable: false),
                    Rotation = table.Column<int>(type: "INTEGER", nullable: false),
                    CenterX = table.Column<int>(type: "INTEGER", nullable: false),
                    CenterY = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<int>(type: "INTEGER", nullable: false),
                    PatternAnimationDtoUuid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatternAnimationSetting", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_PatternAnimationSetting_PatternAnimation_PatternAnimationDtoUuid",
                        column: x => x.PatternAnimationDtoUuid,
                        principalTable: "PatternAnimation",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "AnimationPoint",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    PatternAnimationSettingsUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    RedLaserPowerPwm = table.Column<int>(type: "INTEGER", nullable: false),
                    GreenLaserPowerPwm = table.Column<int>(type: "INTEGER", nullable: false),
                    BlueLaserPowerPwm = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    PatternAnimationSettingsDtoUuid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimationPoint", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_AnimationPoint_PatternAnimationSetting_PatternAnimationSettingsDtoUuid",
                        column: x => x.PatternAnimationSettingsDtoUuid,
                        principalTable: "PatternAnimationSetting",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimationPoint_PatternAnimationSettingsDtoUuid",
                table: "AnimationPoint",
                column: "PatternAnimationSettingsDtoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_LasershowAnimationDto_AnimationUuid",
                table: "LasershowAnimationDto",
                column: "AnimationUuid");

            migrationBuilder.CreateIndex(
                name: "IX_LasershowAnimationDto_LasershowDtoUuid",
                table: "LasershowAnimationDto",
                column: "LasershowDtoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PatternAnimation_AnimationDtoUuid",
                table: "PatternAnimation",
                column: "AnimationDtoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PatternAnimationSetting_PatternAnimationDtoUuid",
                table: "PatternAnimationSetting",
                column: "PatternAnimationDtoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PointDto_PatternDtoUuid",
                table: "PointDto",
                column: "PatternDtoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_ZonesPositionDto_ZoneDtoUuid",
                table: "ZonesPositionDto",
                column: "ZoneDtoUuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimationPoint");

            migrationBuilder.DropTable(
                name: "LasershowAnimationDto");

            migrationBuilder.DropTable(
                name: "PointDto");

            migrationBuilder.DropTable(
                name: "ZonesPositionDto");

            migrationBuilder.DropTable(
                name: "PatternAnimationSetting");

            migrationBuilder.DropTable(
                name: "Lasershow");

            migrationBuilder.DropTable(
                name: "Pattern");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropTable(
                name: "PatternAnimation");

            migrationBuilder.DropTable(
                name: "Animation");
        }
    }
}
