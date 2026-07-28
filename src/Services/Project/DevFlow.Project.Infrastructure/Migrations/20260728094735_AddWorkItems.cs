using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevFlow.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkItems",
                schema: "project",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    AssigneeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReporterId = table.Column<Guid>(type: "uuid", nullable: false),
                    EpicId = table.Column<Guid>(type: "uuid", nullable: true),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    SprintId = table.Column<Guid>(type: "uuid", nullable: true),
                    EstimateHours = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_Key",
                schema: "project",
                table: "WorkItems",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_ProjectId",
                schema: "project",
                table: "WorkItems",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_ProjectId_AssigneeId",
                schema: "project",
                table: "WorkItems",
                columns: new[] { "ProjectId", "AssigneeId" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_ProjectId_Priority",
                schema: "project",
                table: "WorkItems",
                columns: new[] { "ProjectId", "Priority" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_ProjectId_Status",
                schema: "project",
                table: "WorkItems",
                columns: new[] { "ProjectId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkItems",
                schema: "project");
        }
    }
}
