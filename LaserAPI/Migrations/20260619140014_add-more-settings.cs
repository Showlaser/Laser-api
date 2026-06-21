using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    /// <inheritdoc />
    public partial class addmoresettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaxPowerPerlaserInPercentage",
                table: "RegisteredLaser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectionBottomInPercentage",
                table: "RegisteredLaser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectionLeftInPercentage",
                table: "RegisteredLaser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectionRightInPercentage",
                table: "RegisteredLaser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectionTopInPercentage",
                table: "RegisteredLaser",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPowerPerlaserInPercentage",
                table: "RegisteredLaser");

            migrationBuilder.DropColumn(
                name: "ProjectionBottomInPercentage",
                table: "RegisteredLaser");

            migrationBuilder.DropColumn(
                name: "ProjectionLeftInPercentage",
                table: "RegisteredLaser");

            migrationBuilder.DropColumn(
                name: "ProjectionRightInPercentage",
                table: "RegisteredLaser");

            migrationBuilder.DropColumn(
                name: "ProjectionTopInPercentage",
                table: "RegisteredLaser");
        }
    }
}
