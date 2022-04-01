using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatternAnimationDto_Animation_AnimationDtoUuid",
                table: "PatternAnimationDto");

            migrationBuilder.DropForeignKey(
                name: "FK_PatternAnimationSettingsDto_PatternAnimationDto_PatternAnimationDtoUuid",
                table: "PatternAnimationSettingsDto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatternAnimationDto",
                table: "PatternAnimationDto");

            migrationBuilder.RenameTable(
                name: "PatternAnimationDto",
                newName: "PatternAnimation");

            migrationBuilder.RenameIndex(
                name: "IX_PatternAnimationDto_AnimationDtoUuid",
                table: "PatternAnimation",
                newName: "IX_PatternAnimation_AnimationDtoUuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatternAnimation",
                table: "PatternAnimation",
                column: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PatternAnimation_Animation_AnimationDtoUuid",
                table: "PatternAnimation",
                column: "AnimationDtoUuid",
                principalTable: "Animation",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PatternAnimationSettingsDto_PatternAnimation_PatternAnimationDtoUuid",
                table: "PatternAnimationSettingsDto",
                column: "PatternAnimationDtoUuid",
                principalTable: "PatternAnimation",
                principalColumn: "Uuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatternAnimation_Animation_AnimationDtoUuid",
                table: "PatternAnimation");

            migrationBuilder.DropForeignKey(
                name: "FK_PatternAnimationSettingsDto_PatternAnimation_PatternAnimationDtoUuid",
                table: "PatternAnimationSettingsDto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatternAnimation",
                table: "PatternAnimation");

            migrationBuilder.RenameTable(
                name: "PatternAnimation",
                newName: "PatternAnimationDto");

            migrationBuilder.RenameIndex(
                name: "IX_PatternAnimation_AnimationDtoUuid",
                table: "PatternAnimationDto",
                newName: "IX_PatternAnimationDto_AnimationDtoUuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatternAnimationDto",
                table: "PatternAnimationDto",
                column: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PatternAnimationDto_Animation_AnimationDtoUuid",
                table: "PatternAnimationDto",
                column: "AnimationDtoUuid",
                principalTable: "Animation",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PatternAnimationSettingsDto_PatternAnimationDto_PatternAnimationDtoUuid",
                table: "PatternAnimationSettingsDto",
                column: "PatternAnimationDtoUuid",
                principalTable: "PatternAnimationDto",
                principalColumn: "Uuid");
        }
    }
}
