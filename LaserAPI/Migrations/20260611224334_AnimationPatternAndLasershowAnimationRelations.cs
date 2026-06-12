using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    /// <inheritdoc />
    public partial class AnimationPatternAndLasershowAnimationRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PatternAnimation_PatternUuid",
                table: "PatternAnimation",
                column: "PatternUuid");

            migrationBuilder.CreateIndex(
                name: "IX_LasershowAnimation_AnimationUuid",
                table: "LasershowAnimation",
                column: "AnimationUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_LasershowAnimation_Animation_AnimationUuid",
                table: "LasershowAnimation",
                column: "AnimationUuid",
                principalTable: "Animation",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatternAnimation_Pattern_PatternUuid",
                table: "PatternAnimation",
                column: "PatternUuid",
                principalTable: "Pattern",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_LasershowAnimation_AnimationUuid",
                table: "LasershowAnimation");
        }
    }
}
