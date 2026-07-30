using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevFlow.Migrations
{
    /// <inheritdoc />
    public partial class AddWorklogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Worklogs",
                schema: "project",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    StartedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MinutesSpent = table.Column<int>(type: "integer", nullable: false),
                    IsRunning = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worklogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Worklogs_UserId",
                schema: "project",
                table: "Worklogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Worklogs_WorkItemId",
                schema: "project",
                table: "Worklogs",
                column: "WorkItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Worklogs",
                schema: "project");
        }
    }
}
