using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace LaserAPI.Migrations
{
    public partial class SpotifyConnector2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpotifySongId",
                table: "LasershowSpotifyConnector");

            migrationBuilder.CreateTable(
                name: "LasershowSpotifyConnectorSongDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    LasershowSpotifyConnectorUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    SpotifySongId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LasershowSpotifyConnectorSongDto", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_LasershowSpotifyConnectorSongDto_LasershowSpotifyConnector_LasershowSpotifyConnectorUuid",
                        column: x => x.LasershowSpotifyConnectorUuid,
                        principalTable: "LasershowSpotifyConnector",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LasershowSpotifyConnectorSongDto_LasershowSpotifyConnectorUuid",
                table: "LasershowSpotifyConnectorSongDto",
                column: "LasershowSpotifyConnectorUuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LasershowSpotifyConnectorSongDto");

            migrationBuilder.AddColumn<string>(
                name: "SpotifySongId",
                table: "LasershowSpotifyConnector",
                type: "TEXT",
                nullable: true);
        }
    }
}
