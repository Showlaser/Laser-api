using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    /// <inheritdoc />
    public partial class _22032024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatternAnimation_Pattern_PatternUuid",
                table: "PatternAnimation");

            migrationBuilder.AlterColumn<Guid>(
                name: "PatternUuid",
                table: "PatternAnimation",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

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
                name: "LasershowAnimation",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    LasershowUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    AnimationUuid = table.Column<Guid>(type: "TEXT", nullable: true),
                    StartTimeMs = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeLineId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LasershowAnimation", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_LasershowAnimation_Animation_AnimationUuid",
                        column: x => x.AnimationUuid,
                        principalTable: "Animation",
                        principalColumn: "Uuid");
                    table.ForeignKey(
                        name: "FK_LasershowAnimation_Lasershow_LasershowUuid",
                        column: x => x.LasershowUuid,
                        principalTable: "Lasershow",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LasershowAnimation_AnimationUuid",
                table: "LasershowAnimation",
                column: "AnimationUuid");

            migrationBuilder.CreateIndex(
                name: "IX_LasershowAnimation_LasershowUuid",
                table: "LasershowAnimation",
                column: "LasershowUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PatternAnimation_Pattern_PatternUuid",
                table: "PatternAnimation",
                column: "PatternUuid",
                principalTable: "Pattern",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatternAnimation_Pattern_PatternUuid",
                table: "PatternAnimation");

            migrationBuilder.DropTable(
                name: "LasershowAnimation");

            migrationBuilder.DropTable(
                name: "Lasershow");

            migrationBuilder.AlterColumn<Guid>(
                name: "PatternUuid",
                table: "PatternAnimation",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_PatternAnimation_Pattern_PatternUuid",
                table: "PatternAnimation",
                column: "PatternUuid",
                principalTable: "Pattern",
                principalColumn: "Uuid");
        }
    }
}
