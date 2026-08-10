using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailVerificationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailVerificationToken",
                schema: "identity",
                table: "Users",
                column: "EmailVerificationToken",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_EmailVerificationToken",
                schema: "identity",
                table: "Users");
        }
    }
}
