using Microsoft.EntityFrameworkCore.Migrations;

namespace Cde.Migrations
{
    public partial class AuthorWithManyUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Updates_AuthorId",
                table: "Updates");

            migrationBuilder.CreateIndex(
                name: "IX_Updates_AuthorId",
                table: "Updates",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Updates_AuthorId",
                table: "Updates");

            migrationBuilder.CreateIndex(
                name: "IX_Updates_AuthorId",
                table: "Updates",
                column: "AuthorId",
                unique: true);
        }
    }
}
