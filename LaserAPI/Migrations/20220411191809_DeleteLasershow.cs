using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    public partial class DeleteLasershow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LasershowAnimation");

            migrationBuilder.DropTable(
                name: "Lasershow");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "LasershowAnimation",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnimationUuid = table.Column<Guid>(type: "TEXT", nullable: true),
                    LasershowUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    StartTimeOffset = table.Column<int>(type: "INTEGER", nullable: false),
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
        }
    }
}
