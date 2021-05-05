using Microsoft.EntityFrameworkCore.Migrations;

namespace Cde.Migrations
{
    public partial class UpdateAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Updates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Updates_AuthorId",
                table: "Updates",
                column: "AuthorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Updates_AspNetUsers_AuthorId",
                table: "Updates",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Updates_AspNetUsers_AuthorId",
                table: "Updates");

            migrationBuilder.DropIndex(
                name: "IX_Updates_AuthorId",
                table: "Updates");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Updates");
        }
    }
}
