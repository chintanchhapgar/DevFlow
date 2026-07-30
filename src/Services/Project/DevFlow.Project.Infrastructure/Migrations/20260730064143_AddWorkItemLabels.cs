using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevFlow.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkItemLabels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkItemLabels",
                schema: "project",
                columns: table => new
                {
                    WorkItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    LabelId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkItemAggregateId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItemLabels", x => new { x.WorkItemId, x.LabelId });
                    table.ForeignKey(
                        name: "FK_WorkItemLabels_WorkItems_WorkItemAggregateId",
                        column: x => x.WorkItemAggregateId,
                        principalSchema: "project",
                        principalTable: "WorkItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemLabels_WorkItemAggregateId",
                schema: "project",
                table: "WorkItemLabels",
                column: "WorkItemAggregateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkItemLabels",
                schema: "project");
        }
    }
}
