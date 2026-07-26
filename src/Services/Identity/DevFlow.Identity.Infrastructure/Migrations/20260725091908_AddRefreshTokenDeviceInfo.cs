using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevFlow.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenDeviceInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Browser",
                schema: "identity",
                table: "RefreshTokens",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                schema: "identity",
                table: "RefreshTokens",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                schema: "identity",
                table: "RefreshTokens",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUsedOnUtc",
                schema: "identity",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatingSystem",
                schema: "identity",
                table: "RefreshTokens",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                schema: "identity",
                table: "RefreshTokens",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Browser",
                schema: "identity",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                schema: "identity",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                schema: "identity",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "LastUsedOnUtc",
                schema: "identity",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "OperatingSystem",
                schema: "identity",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                schema: "identity",
                table: "RefreshTokens");
        }
    }
}
