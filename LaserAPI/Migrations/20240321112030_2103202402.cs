using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    /// <inheritdoc />
    public partial class _2103202402 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZonePosition_Zone_ZoneUuid",
                table: "ZonePosition");

            migrationBuilder.RenameColumn(
                name: "ZoneUuid",
                table: "ZonePosition",
                newName: "SafetyZoneUuid");

            migrationBuilder.RenameIndex(
                name: "IX_ZonePosition_ZoneUuid",
                table: "ZonePosition",
                newName: "IX_ZonePosition_SafetyZoneUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_ZonePosition_Zone_SafetyZoneUuid",
                table: "ZonePosition",
                column: "SafetyZoneUuid",
                principalTable: "Zone",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZonePosition_Zone_SafetyZoneUuid",
                table: "ZonePosition");

            migrationBuilder.RenameColumn(
                name: "SafetyZoneUuid",
                table: "ZonePosition",
                newName: "ZoneUuid");

            migrationBuilder.RenameIndex(
                name: "IX_ZonePosition_SafetyZoneUuid",
                table: "ZonePosition",
                newName: "IX_ZonePosition_ZoneUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_ZonePosition_Zone_ZoneUuid",
                table: "ZonePosition",
                column: "ZoneUuid",
                principalTable: "Zone",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
