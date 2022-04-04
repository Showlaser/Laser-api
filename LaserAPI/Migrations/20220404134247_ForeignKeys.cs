using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaserAPI.Migrations
{
    public partial class ForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimationPoint_PatternAnimationSetting_PatternAnimationSettingsDtoUuid",
                table: "AnimationPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_PatternAnimation_Animation_AnimationDtoUuid",
                table: "PatternAnimation");

            migrationBuilder.DropForeignKey(
                name: "FK_PatternAnimationSetting_PatternAnimation_PatternAnimationDtoUuid",
                table: "PatternAnimationSetting");

            migrationBuilder.DropIndex(
                name: "IX_PatternAnimationSetting_PatternAnimationDtoUuid",
                table: "PatternAnimationSetting");

            migrationBuilder.DropIndex(
                name: "IX_PatternAnimation_AnimationDtoUuid",
                table: "PatternAnimation");

            migrationBuilder.DropIndex(
                name: "IX_AnimationPoint_PatternAnimationSettingsDtoUuid",
                table: "AnimationPoint");

            migrationBuilder.DropColumn(
                name: "PatternAnimationDtoUuid",
                table: "PatternAnimationSetting");

            migrationBuilder.DropColumn(
                name: "AnimationDtoUuid",
                table: "PatternAnimation");

            migrationBuilder.DropColumn(
                name: "PatternAnimationSettingsDtoUuid",
                table: "AnimationPoint");

            migrationBuilder.CreateIndex(
                name: "IX_PatternAnimationSetting_PatternAnimationUuid",
                table: "PatternAnimationSetting",
                column: "PatternAnimationUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PatternAnimation_AnimationUuid",
                table: "PatternAnimation",
                column: "AnimationUuid");

            migrationBuilder.CreateIndex(
                name: "IX_AnimationPoint_PatternAnimationSettingsUuid",
                table: "AnimationPoint",
                column: "PatternAnimationSettingsUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimationPoint_PatternAnimationSetting_PatternAnimationSettingsUuid",
                table: "AnimationPoint",
                column: "PatternAnimationSettingsUuid",
                principalTable: "PatternAnimationSetting",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatternAnimation_Animation_AnimationUuid",
                table: "PatternAnimation",
                column: "AnimationUuid",
                principalTable: "Animation",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatternAnimationSetting_PatternAnimation_PatternAnimationUuid",
                table: "PatternAnimationSetting",
                column: "PatternAnimationUuid",
                principalTable: "PatternAnimation",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimationPoint_PatternAnimationSetting_PatternAnimationSettingsUuid",
                table: "AnimationPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_PatternAnimation_Animation_AnimationUuid",
                table: "PatternAnimation");

            migrationBuilder.DropForeignKey(
                name: "FK_PatternAnimationSetting_PatternAnimation_PatternAnimationUuid",
                table: "PatternAnimationSetting");

            migrationBuilder.DropIndex(
                name: "IX_PatternAnimationSetting_PatternAnimationUuid",
                table: "PatternAnimationSetting");

            migrationBuilder.DropIndex(
                name: "IX_PatternAnimation_AnimationUuid",
                table: "PatternAnimation");

            migrationBuilder.DropIndex(
                name: "IX_AnimationPoint_PatternAnimationSettingsUuid",
                table: "AnimationPoint");

            migrationBuilder.AddColumn<Guid>(
                name: "PatternAnimationDtoUuid",
                table: "PatternAnimationSetting",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AnimationDtoUuid",
                table: "PatternAnimation",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PatternAnimationSettingsDtoUuid",
                table: "AnimationPoint",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatternAnimationSetting_PatternAnimationDtoUuid",
                table: "PatternAnimationSetting",
                column: "PatternAnimationDtoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PatternAnimation_AnimationDtoUuid",
                table: "PatternAnimation",
                column: "AnimationDtoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_AnimationPoint_PatternAnimationSettingsDtoUuid",
                table: "AnimationPoint",
                column: "PatternAnimationSettingsDtoUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimationPoint_PatternAnimationSetting_PatternAnimationSettingsDtoUuid",
                table: "AnimationPoint",
                column: "PatternAnimationSettingsDtoUuid",
                principalTable: "PatternAnimationSetting",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PatternAnimation_Animation_AnimationDtoUuid",
                table: "PatternAnimation",
                column: "AnimationDtoUuid",
                principalTable: "Animation",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PatternAnimationSetting_PatternAnimation_PatternAnimationDtoUuid",
                table: "PatternAnimationSetting",
                column: "PatternAnimationDtoUuid",
                principalTable: "PatternAnimation",
                principalColumn: "Uuid");
        }
    }
}
