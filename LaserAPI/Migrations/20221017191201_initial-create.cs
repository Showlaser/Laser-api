using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace LaserAPI.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    MaxLaserPowerInZonePwm = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zone", x => x.Uuid);
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
                    ConnectedToPointOrderNr = table.Column<int>(type: "INTEGER", nullable: true),
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
                    ZoneUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZonePosition", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_ZonePosition_Zone_ZoneUuid",
                        column: x => x.ZoneUuid,
                        principalTable: "Zone",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animation",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PointUuid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animation", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Animation_Point_PointUuid",
                        column: x => x.PointUuid,
                        principalTable: "Point",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "PatternAnimation",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnimationUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartTimeOffset = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeLineId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "PatternAnimationSetting",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    PatternAnimationUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Scale = table.Column<double>(type: "REAL", nullable: false),
                    Rotation = table.Column<int>(type: "INTEGER", nullable: false),
                    CenterX = table.Column<int>(type: "INTEGER", nullable: false),
                    CenterY = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatternAnimationSetting", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_PatternAnimationSetting_PatternAnimation_PatternAnimationUuid",
                        column: x => x.PatternAnimationUuid,
                        principalTable: "PatternAnimation",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
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
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimationPoint", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_AnimationPoint_PatternAnimationSetting_PatternAnimationSettingsUuid",
                        column: x => x.PatternAnimationSettingsUuid,
                        principalTable: "PatternAnimationSetting",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animation_PointUuid",
                table: "Animation",
                column: "PointUuid");

            migrationBuilder.CreateIndex(
                name: "IX_AnimationPoint_PatternAnimationSettingsUuid",
                table: "AnimationPoint",
                column: "PatternAnimationSettingsUuid");

            migrationBuilder.CreateIndex(
                name: "IX_LasershowSpotifyConnectorSongDto_LasershowSpotifyConnectorUuid",
                table: "LasershowSpotifyConnectorSongDto",
                column: "LasershowSpotifyConnectorUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PatternAnimation_AnimationUuid",
                table: "PatternAnimation",
                column: "AnimationUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PatternAnimationSetting_PatternAnimationUuid",
                table: "PatternAnimationSetting",
                column: "PatternAnimationUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Point_PatternUuid",
                table: "Point",
                column: "PatternUuid");

            migrationBuilder.CreateIndex(
                name: "IX_ZonePosition_ZoneUuid",
                table: "ZonePosition",
                column: "ZoneUuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimationPoint");

            migrationBuilder.DropTable(
                name: "LasershowSpotifyConnectorSongDto");

            migrationBuilder.DropTable(
                name: "ZonePosition");

            migrationBuilder.DropTable(
                name: "PatternAnimationSetting");

            migrationBuilder.DropTable(
                name: "LasershowSpotifyConnector");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropTable(
                name: "PatternAnimation");

            migrationBuilder.DropTable(
                name: "Animation");

            migrationBuilder.DropTable(
                name: "Point");

            migrationBuilder.DropTable(
                name: "Pattern");
        }
    }
}
