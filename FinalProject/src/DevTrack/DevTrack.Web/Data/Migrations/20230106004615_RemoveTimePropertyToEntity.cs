﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTrack.Web.Data.Migrations
{
    public partial class RemoveTimePropertyToEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "WebcamCaptures");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "ScreenCaptures");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "RunningPrograms");

            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "ActiveWindows");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "WebcamCaptures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "ScreenCaptures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "RunningPrograms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "Activities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "ActiveWindows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}