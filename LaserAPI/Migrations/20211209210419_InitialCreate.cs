﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pattern",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Scale = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pattern", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "PointDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    PatternUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    X = table.Column<short>(type: "INTEGER", nullable: false),
                    Y = table.Column<short>(type: "INTEGER", nullable: false),
                    ConnectedToUuid = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointDto", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_PointDto_Pattern_PatternUuid",
                        column: x => x.PatternUuid,
                        principalTable: "Pattern",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointDto_PatternUuid",
                table: "PointDto",
                column: "PatternUuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointDto");

            migrationBuilder.DropTable(
                name: "Pattern");
        }
    }
}