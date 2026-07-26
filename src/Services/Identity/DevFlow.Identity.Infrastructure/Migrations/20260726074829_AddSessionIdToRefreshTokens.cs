using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevFlow.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionIdToRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SessionId",
                schema: "identity",
                table: "RefreshTokens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_SessionId",
                schema: "identity",
                table: "RefreshTokens",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_SessionId",
                schema: "identity",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "SessionId",
                schema: "identity",
                table: "RefreshTokens");
        }
    }
}
