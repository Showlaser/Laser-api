using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    /// <inheritdoc />
    public partial class _23032024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeLineId",
                table: "PatternAnimation",
                newName: "TimelineId");

            migrationBuilder.RenameColumn(
                name: "TimeLineId",
                table: "LasershowAnimation",
                newName: "TimelineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimelineId",
                table: "PatternAnimation",
                newName: "TimeLineId");

            migrationBuilder.RenameColumn(
                name: "TimelineId",
                table: "LasershowAnimation",
                newName: "TimeLineId");
        }
    }
}
