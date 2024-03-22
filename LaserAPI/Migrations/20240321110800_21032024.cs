using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    /// <inheritdoc />
    public partial class _21032024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animation_Point_PointUuid",
                table: "Animation");

            migrationBuilder.DropTable(
                name: "AnimationPoint");

            migrationBuilder.DropTable(
                name: "PatternAnimationSetting");

            migrationBuilder.DropIndex(
                name: "IX_Animation_PointUuid",
                table: "Animation");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "ZonePosition",
                newName: "OrderNr");

            migrationBuilder.RenameColumn(
                name: "MaxLaserPowerInZonePwm",
                table: "Zone",
                newName: "MaxLaserPowerInZonePercentage");

            migrationBuilder.RenameColumn(
                name: "StartTimeOffset",
                table: "PatternAnimation",
                newName: "StartTimeMs");

            migrationBuilder.RenameColumn(
                name: "PointUuid",
                table: "Animation",
                newName: "Image");

            migrationBuilder.AddColumn<Guid>(
                name: "PatternUuid",
                table: "PatternAnimation",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnimationPatternKeyFrameDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnimationPatternUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimeMs = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyEdited = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyValue = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "IX_PatternAnimation_PatternUuid",
                table: "PatternAnimation",
                column: "PatternUuid");

            migrationBuilder.CreateIndex(
                name: "IX_AnimationPatternKeyFrameDto_AnimationPatternUuid",
                table: "AnimationPatternKeyFrameDto",
                column: "AnimationPatternUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PatternAnimation_Pattern_PatternUuid",
                table: "PatternAnimation",
                column: "PatternUuid",
                principalTable: "Pattern",
                principalColumn: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatternAnimation_Pattern_PatternUuid",
                table: "PatternAnimation");

            migrationBuilder.DropTable(
                name: "AnimationPatternKeyFrameDto");

            migrationBuilder.DropIndex(
                name: "IX_PatternAnimation_PatternUuid",
                table: "PatternAnimation");

            migrationBuilder.DropColumn(
                name: "PatternUuid",
                table: "PatternAnimation");

            migrationBuilder.RenameColumn(
                name: "OrderNr",
                table: "ZonePosition",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "MaxLaserPowerInZonePercentage",
                table: "Zone",
                newName: "MaxLaserPowerInZonePwm");

            migrationBuilder.RenameColumn(
                name: "StartTimeMs",
                table: "PatternAnimation",
                newName: "StartTimeOffset");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Animation",
                newName: "PointUuid");

            migrationBuilder.CreateTable(
                name: "PatternAnimationSetting",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    CenterX = table.Column<int>(type: "INTEGER", nullable: false),
                    CenterY = table.Column<int>(type: "INTEGER", nullable: false),
                    PatternAnimationUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Rotation = table.Column<int>(type: "INTEGER", nullable: false),
                    Scale = table.Column<double>(type: "REAL", nullable: false),
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
                    BlueLaserPowerPwm = table.Column<int>(type: "INTEGER", nullable: false),
                    GreenLaserPowerPwm = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    PatternAnimationSettingsUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    RedLaserPowerPwm = table.Column<int>(type: "INTEGER", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "IX_PatternAnimationSetting_PatternAnimationUuid",
                table: "PatternAnimationSetting",
                column: "PatternAnimationUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Animation_Point_PointUuid",
                table: "Animation",
                column: "PointUuid",
                principalTable: "Point",
                principalColumn: "Uuid");
        }
    }
}
