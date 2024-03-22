using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    /// <inheritdoc />
    public partial class _2103202404 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppliedOnShowlaserUuid",
                table: "Zone",
                newName: "AppliedOnShowLaserUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppliedOnShowLaserUuid",
                table: "Zone",
                newName: "AppliedOnShowlaserUuid");
        }
    }
}
