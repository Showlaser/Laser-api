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
                name: "PatternAnimationSettingsDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnimationTimelineUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Scale = table.Column<double>(type: "REAL", nullable: false),
                    CenterX = table.Column<int>(type: "INTEGER", nullable: false),
                    CenterY = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatternAnimationSettingsDto", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "PointDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    PatternUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointDto", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_PointDto_Pattern_PatternUuid",
                        column: x => x.PatternUuid,
                        principalTable: "Pattern",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimationPointDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimelineSettingsUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimationPointDto", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_AnimationPointDto_PatternAnimationSettingsDto_TimelineSettingsUuid",
                        column: x => x.TimelineSettingsUuid,
                        principalTable: "PatternAnimationSettingsDto",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatternAnimationDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnimationUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnimationSettingsUuid = table.Column<Guid>(type: "TEXT", nullable: true),
                    TimeLineId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatternAnimationDto", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_PatternAnimationDto_Animation_AnimationUuid",
                        column: x => x.AnimationUuid,
                        principalTable: "Animation",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatternAnimationDto_PatternAnimationSettingsDto_AnimationSettingsUuid",
                        column: x => x.AnimationSettingsUuid,
                        principalTable: "PatternAnimationSettingsDto",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimationPointDto_TimelineSettingsUuid",
                table: "AnimationPointDto",
                column: "TimelineSettingsUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PatternAnimationDto_AnimationSettingsUuid",
                table: "PatternAnimationDto",
                column: "AnimationSettingsUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PatternAnimationDto_AnimationUuid",
                table: "PatternAnimationDto",
                column: "AnimationUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PointDto_PatternUuid",
                table: "PointDto",
                column: "PatternUuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimationPointDto");

            migrationBuilder.DropTable(
                name: "PatternAnimationDto");

            migrationBuilder.DropTable(
                name: "PointDto");

            migrationBuilder.DropTable(
                name: "Animation");

            migrationBuilder.DropTable(
                name: "PatternAnimationSettingsDto");

            migrationBuilder.DropTable(
                name: "Pattern");
        }
    }
}
