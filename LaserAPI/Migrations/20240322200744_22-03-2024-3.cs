using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    /// <inheritdoc />
    public partial class _220320243 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LasershowAnimation_Animation_AnimationUuid",
                table: "LasershowAnimation");

            migrationBuilder.DropIndex(
                name: "IX_LasershowAnimation_AnimationUuid",
                table: "LasershowAnimation");

            migrationBuilder.AlterColumn<double>(
                name: "PropertyValue",
                table: "AnimationPatternKeyFrameDto",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PropertyValue",
                table: "AnimationPatternKeyFrameDto",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
