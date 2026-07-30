using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevFlow.Migrations
{
    /// <inheritdoc />
    public partial class AddAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachments",
                schema: "project",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    UploadedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalFileName = table.Column<string>(type: "character varying(260)", maxLength: 260, nullable: false),
                    StoredFileName = table.Column<string>(type: "character varying(260)", maxLength: 260, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Extension = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    StoragePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_WorkItemId",
                schema: "project",
                table: "Attachments",
                column: "WorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_WorkItemId_CreatedOnUtc",
                schema: "project",
                table: "Attachments",
                columns: new[] { "WorkItemId", "CreatedOnUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments",
                schema: "project");
        }
    }
}
