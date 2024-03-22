using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    /// <inheritdoc />
    public partial class _220320242 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LasershowAnimation_Animation_AnimationUuid",
                table: "LasershowAnimation");

            migrationBuilder.DropForeignKey(
                name: "FK_PatternAnimation_Pattern_PatternUuid",
                table: "PatternAnimation");

            migrationBuilder.DropIndex(
                name: "IX_PatternAnimation_PatternUuid",
                table: "PatternAnimation");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnimationUuid",
                table: "LasershowAnimation",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LasershowAnimation_Animation_AnimationUuid",
                table: "LasershowAnimation",
                column: "AnimationUuid",
                principalTable: "Animation",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LasershowAnimation_Animation_AnimationUuid",
                table: "LasershowAnimation");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnimationUuid",
                table: "LasershowAnimation",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_PatternAnimation_PatternUuid",
                table: "PatternAnimation",
                column: "PatternUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_LasershowAnimation_Animation_AnimationUuid",
                table: "LasershowAnimation",
                column: "AnimationUuid",
                principalTable: "Animation",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PatternAnimation_Pattern_PatternUuid",
                table: "PatternAnimation",
                column: "PatternUuid",
                principalTable: "Pattern",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
